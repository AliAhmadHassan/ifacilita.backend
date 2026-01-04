using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocusignController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Task.FromResult(new { code=200, message=$" [ {DateTime.Now.ToString("dd/MM/yyyy")} ] - Saúde do Endpoint está ok" }));
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
