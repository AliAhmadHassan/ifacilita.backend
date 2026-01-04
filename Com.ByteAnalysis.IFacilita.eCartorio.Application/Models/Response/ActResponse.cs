using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response
{
    public class ActResponse
    {
        public long NumeroAto { get; set; }

        public string Status { get; set; }

        public string Certidao { get; set; }

        public string Cartorio { get; set; }

        public string Finalidade { get; set; }

        public DateTime? DataPagamento { get; set; }

        public string Cerp { get; set; }

        public string TipoAto { get; set; }

        public int IdItem { get; set; }

        public string UrlAct { get; set; }

        public OrderInfoResponse InformacaoPedido { get; set; }
    }

    public class ActDto
    {
        public long ActNumber { get; set; }

        public string Status { get; set; }

        public string Certificate { get; set; }

        public string Registry { get; set; }

        public string Goal { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string Cerp { get; set; }

        public string ActType { get; set; }

        public int IdItem { get; set; }

        public string UrlAct { get; set; }

        public OrderInfoResponse OrderDetail { get; set; }
    }


}
