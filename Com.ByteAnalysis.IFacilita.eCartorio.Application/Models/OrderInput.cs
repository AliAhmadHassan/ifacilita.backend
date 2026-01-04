using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models
{
    public class OrderInput : BaseInput
    {
        public long NumeroPedido { get; set; }

        public DateTime DataPedido { get; set; }

        public int QuantidadeAtos { get; set; }

        public decimal ValorPedido { get; set; }

        public RequerenteInput Requerente { get; set; }

        public IEnumerable<AtoInput> Atos { get; set; }

        public string AtosCompactados { get; set; }

        public string Billet { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

    }

    public class OrderInputDto : BaseInput
    {
        public long OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public int QuantityActs { get; set; }

        public decimal OrderValue { get; set; }

        public ApplicantInputDto Applicant { get; set; }

        public IEnumerable<ActInputDto> Acts { get; set; }

        public string UrlActsZip { get; set; }

        public string Billet { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

    }
}
