using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.ByteAnalysis.IFacilita.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionCertificationController : ControllerBase
    {
        Service.ITransactionCertificationService service;

        List<Entity.TransactionCertification> transactionCertifications = new List<Entity.TransactionCertification>()
        {
            new Entity.TransactionCertification(){CertificateName="a) Certidões Negativas expedidas pelo 1º,2º,3º e 4º Ofício de Distribuição;",
            CertificateFilename="Certidões Negativas expedidas pelo 1º,2º,3º e 4º Ofício de Distribuição.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\Certidões Negativas expedidas pelo 1º,2º,3º e 4º Ofício de Distribuição.pdf"},

            new Entity.TransactionCertification(){CertificateName="b) Certidões Negativas expedidas pelo 1º e 2º Ofícios de Interdições e Tutelas;",
            CertificateFilename="Certidões Negativas expedidas pelo 1º e 2º Ofícios de Interdições e Tutelas.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\Certidões Negativas expedidas pelo 1º e 2º Ofícios de Interdições e Tutelas.pdf"},

            new Entity.TransactionCertification(){CertificateName="e) Certidão de ônus reais do IMÓVEL com a propriedade em nome da Promitente Vendedora;",
            CertificateFilename="CERTIDÃO 706.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\CERTIDÃO 706.pdf"},

            new Entity.TransactionCertification(){CertificateName="f) Certidão Negativa da Prefeitura do Rio de Janeiro, relativa ao IMÓVEL;",
            CertificateFilename="CERTIDÕES DADU.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\CERTIDÕES DADU.pdf"},

            new Entity.TransactionCertification(){CertificateName="g) Certidão Negativa de Taxa de Incêndio, relativa ao IMÓVEL;",
            CertificateFilename="CERTIDÃO 706.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\CERTIDÃO 706.pdf"},

            new Entity.TransactionCertification(){CertificateName="h) Declaração de quitação do condomínio.",
            CertificateFilename="CERTIDÕES DADU.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\CERTIDÕES DADU.pdf"},

            new Entity.TransactionCertification(){CertificateName="i) IPTU prefeitura.",
            CertificateFilename="CERTIDÃO 706.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\CERTIDÃO 706.pdf"},

            new Entity.TransactionCertification(){CertificateName="c) Certidão de distribuição e registro da Justiça Federal e CNDT;",
            CertificateFilename="Certidão de distribuição e registro da Justiça Federal e CNDT.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\Certidão de distribuição e registro da Justiça Federal e CNDT.pdf"},







            new Entity.TransactionCertification(){CertificateName="k) Certidões Eletrônicas - Justiça Federal.",
            CertificateFilename="CERTIDÃO 706.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\CERTIDÃO 706.pdf"},
            
            new Entity.TransactionCertification(){CertificateName="d) Certidão de situação fiscal e enfitêutica do IMÓVEL;",
            CertificateFilename="Certidão de situação fiscal e enfitêutica do IMÓVEL.pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\Certidão de situação fiscal e enfitêutica do IMÓVEL.pdf"},

            new Entity.TransactionCertification(){CertificateName="j) Certidão Negativa de Débitos Trabalhistas.",
            CertificateFilename="Certidão Negativa de Débitos Trabalhistas..pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\Certidão Negativa de Débitos Trabalhistas..pdf"},

            new Entity.TransactionCertification(){CertificateName="l) Certidão do bombeiro - FUNESBOM.",
            CertificateFilename="Certidão Negativa de Débitos Trabalhistas..pdf",
            CertificatePath = @"C:\Users\haley\source\repos\byteanalysis\Ifacilita\Project\ifacilita-frontend\src\assets\pdfs\Certidão Negativa de Débitos Trabalhistas..pdf"},

        };

        public TransactionCertificationController(Service.ITransactionCertificationService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get(int idtransaction)
        {
            return Ok(this.transactionCertifications);
        }

        [HttpGet("{idtransaction}/get-list-certification-for-upload")]
        public IActionResult GetCertificationListForUploadDocument(int idtransaction)
        {
            return Ok(this.service.GetCertificationListForUploadDocument(idtransaction));
        }

        [HttpGet("{idtransaction}/get-list-certification")]
        public IActionResult GetListCertification(int idtransaction)
        {
            return Ok(this.service.GetListCertification(idtransaction));
        }

        [HttpGet("{idtransaction}/idtransaction")]
        public IActionResult GetByIdtransaction(int idtransaction)
        {
            return Ok(this.service.FindByIdtransaction(idtransaction));
        }


        [HttpGet("{idtransaction}")]
        public IActionResult GetById(int id)
        {
            return Ok(this.service.FindById(id));
        }

        // POST: api/TransactionCertification
        [HttpPost("create-certification")]
        public IActionResult PostCreateCertification([FromBody] Entity.TransactionCertification entity)
        {
            var resultCertifications = this.service.FindByIdtransaction(entity.Idtransaction);
            List<Entity.TransactionCertification> transactionCertifications = resultCertifications.Where(c => c.EcartorioId.Equals("Ifacilita.RPA")).ToList();

            foreach (var item in transactionCertifications)
            {
                this.service.RequestIFacilitaRPA(item);
            }

            return Ok(transactionCertifications);
        }

        // POST: api/TransactionCertification
        [HttpPost]
        public IActionResult Post([FromBody] Entity.TransactionCertification entity)
        {
            return Ok(this.service.Insert(entity));
        }

        [HttpPost("{certificationName}/upload")]
        public IActionResult Post(string certificationName, [FromBody] Entity.TransactionCertification entity)
        {
            this.service.UploadCertification(certificationName, entity);
            return Ok();
        }

        [HttpPost("{idTransaction}/request")]
        public IActionResult MakeECartorioRequest(int idTransaction)
        {
            return Ok(this.service.MakeECartorioRequest(idTransaction));
        }

        // PUT: api/TransactionCertification/5
        [HttpPut]
        public IActionResult Put([FromBody] Entity.TransactionCertification entity)
        {
            return Ok(this.service.Update(entity));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.service.Delete(id);

            return Ok();
        }
    }
}
