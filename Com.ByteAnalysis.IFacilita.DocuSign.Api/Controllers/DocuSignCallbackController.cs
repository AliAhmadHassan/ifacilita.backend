using Com.ByteAnalysis.IFacilita.Common;
using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Com.ByteAnalysis.IFacilita.DocuSign.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocuSignCallbackController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDocuSignApi<Authentication> _docuSignApi;
        private readonly IEnvelopeDocuSignService _envelopeDocuSignService;

        private IS3 s3;

        public DocuSignCallbackController(
            IHttpContextAccessor httpContextAccessor,
            IAuthenticationService authenticationService,
            IDocuSignApi<Authentication> docuSignApi,
            IEnvelopeDocuSignService envelopeDocuSignService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
            _docuSignApi = docuSignApi;
            _envelopeDocuSignService = envelopeDocuSignService;

            this.s3 = new Common.Impl.S3();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var code = Request.Query["code"].ToString();

            _ = await _authenticationService.CreateAsync(new Authentication() { Code = code, Created = System.DateTime.Now });
            _ = await _docuSignApi.GenerateTokenAccess();

            return Ok(new { code = 200, message = "Token capturado com sucesso e autenticação no Docusign realizada" });
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                await OutputLog("Obtendo callback do Docusign");

                Stream stream = Request.Body;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(stream);

                var mgr = new XmlNamespaceManager(xmldoc.NameTable);
                mgr.AddNamespace("a", "http://www.docusign.net/API/3.0");

                XmlNode envelopeStatus = xmldoc.SelectSingleNode("//a:EnvelopeStatus", mgr);
                XmlNode envelopeId = envelopeStatus.SelectSingleNode("//a:EnvelopeID", mgr);
                XmlNode status = envelopeStatus.SelectSingleNode("./a:Status", mgr);

                await OutputLog("Chave do envelope que está respondendo: " + envelopeId.InnerText);
                await OutputLog("Status reportado: " + status.InnerText);

                var targetFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docusign");
                if (!Directory.Exists(targetFileDirectory))
                    Directory.CreateDirectory(targetFileDirectory);

                await OutputLog("Gravando arquivo XML");
                if (envelopeId != null)
                    System.IO.File.WriteAllText($"{targetFileDirectory}/{envelopeId.InnerText}_{status.InnerText}_.xml", xmldoc.OuterXml);

                if (status.InnerText == "Completed")
                {
                    // Loop through the DocumentPDFs element, storing each document.
                    await OutputLog("Gravando arquivo(s) PDF");

                    XmlNode docs = xmldoc.SelectSingleNode("//a:DocumentPDFs", mgr);
                    foreach (XmlNode doc in docs.ChildNodes)
                    {
                        string documentName = doc.ChildNodes[0].InnerText; // pdf.SelectSingleNode("//a:Name", mgr).InnerText;
                        string documentId = doc.ChildNodes[2].InnerText; // pdf.SelectSingleNode("//a:DocumentID", mgr).InnerText;
                        string byteStr = doc.ChildNodes[1].InnerText; // pdf.SelectSingleNode("//a:PDFBytes", mgr).InnerText;

                        System.IO.File.WriteAllText($"{targetFileDirectory}/{envelopeId.InnerText}_{documentId}_{documentName}", byteStr);
                    }
                }
            }
            catch (Exception ex)
            {
                await OutputLog("[ERRO] - Houve uma falha ao tentar ler os dados da requisição. A mensagem do servidor foi: " + ex.Message);
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("ConnectWebHook")]
        public async Task<IActionResult> ConnectWebHook()
        {
            try
            {
                await OutputLog("Obtendo callback do Docusign");

                Stream stream = Request.Body;

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(stream);

                var mgr = new XmlNamespaceManager(xmldoc.NameTable);
                mgr.AddNamespace("a", "http://www.docusign.net/API/3.0");

                XmlNode envelopeStatus = xmldoc.SelectSingleNode("//a:EnvelopeStatus", mgr);
                XmlNode envelopeId = envelopeStatus.SelectSingleNode("//a:EnvelopeID", mgr);
                XmlNode status = envelopeStatus.SelectSingleNode("./a:Status", mgr);

                var envelopeLocal = await _envelopeDocuSignService.GetByEnvelopeIdAsync(envelopeId.InnerText);
                if (envelopeLocal == null)
                {
                    await OutputLog("O envelope entrante não foi encontrado na base de dados. Envelope ID: " + envelopeId.InnerText);
                    return Ok();
                }

                await OutputLog("Id do banco local: " + envelopeLocal.Id);
                await OutputLog("Chave do envelope que está respondendo: " + envelopeId.InnerText);
                await OutputLog("Status reportado: " + status.InnerText);

                var targetFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docusign");
                if (!Directory.Exists(targetFileDirectory))
                    Directory.CreateDirectory(targetFileDirectory);

                /* await OutputLog("Gravando arquivo XML");
                 if (envelopeId != null)
                     System.IO.File.WriteAllText($"{targetFileDirectory}/{envelopeId.InnerText}_{status.InnerText}_.xml", xmldoc.OuterXml);
                */

                if (envelopeLocal.DocumentsResponse == null)
                    envelopeLocal.DocumentsResponse = new List<DocumentReponse>();

                await OutputLog("Gravando arquivo XML no S3");

                envelopeLocal.DocumentsResponse.Add(new DocumentReponse()
                {
                    DateReceived = DateTime.Now,
                    Status = status.InnerText,
                    Url = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(xmldoc.OuterXml)), ".xml")
                });

                await OutputLog("Arquivo XML gravado com sucesso");

                if (status.InnerText == "Completed")
                {
                    // Loop through the DocumentPDFs element, storing each document.
                    await OutputLog("Gravando arquivo(s) PDF");

                    XmlNode docs = xmldoc.SelectSingleNode("//a:DocumentPDFs", mgr);
                    foreach (XmlNode doc in docs.ChildNodes)
                    {
                        string documentName = doc.ChildNodes[0].InnerText; // pdf.SelectSingleNode("//a:Name", mgr).InnerText;
                        string documentId = doc.ChildNodes[2].InnerText; // pdf.SelectSingleNode("//a:DocumentID", mgr).InnerText;
                        string byteStr = doc.ChildNodes[1].InnerText; // pdf.SelectSingleNode("//a:PDFBytes", mgr).InnerText;

                        //System.IO.File.WriteAllText($"{targetFileDirectory}/{envelopeId.InnerText}_{documentId}_{documentName}", byteStr);

                        await OutputLog("Gravando arquivo PDF no S3: " + documentName);

                        envelopeLocal.DocumentsResponse.Add(new DocumentReponse()
                        {
                            FileName = documentName,
                            DateReceived = DateTime.Now,
                            Status = status.InnerText,
                            Url = "https://ifacilita.s3.us-east-2.amazonaws.com/" + this.s3.SaveFile(byteStr, ".pdf")
                        });

                        await OutputLog("Arquivo PDF gravado com sucesso");
                    }
                }

                await OutputLog("Atualizando status do envelope para: " + status.InnerText);
                envelopeLocal.Status = status.InnerText;
                await _envelopeDocuSignService.UpdateAsync(envelopeLocal.Id, envelopeLocal);

                await OutputLog("Status atualizado com sucesso.");
                await OutputLog("Processo finalizado com sucesso.");
            }
            catch (Exception ex)
            {
                await OutputLog("[ERRO] - Houve uma falha ao tentar ler os dados da requisição. A mensagem do servidor foi: " + ex.Message);
                return BadRequest();
            }

            return Ok();
        }

        private Task<bool> OutputLog(string msg)
        {
            try
            {
                msg = $"[ {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff")} ] - {msg}";

                if (!Directory.Exists("Logs"))
                    Directory.CreateDirectory("Logs");

                System.IO.File.AppendAllText($"Logs/DocuSign-Log-{DateTime.Now.ToString("dd-MM-yyyy")}.txt", msg);

                Console.WriteLine(msg);
            }
            catch { }

            return Task.FromResult(true);
        }
    }
}