using System;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response
{
    public class OrderInfoResponse
    {
        public long NumeroPedido { get; set; }

        public DateTime DataPedido { get; set; }

        public ApplicantInfoResponse InfoRequerente { get; set; }
    }

    public class OrderDetailDto
    {
        public long OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public ApplicantInfoResponse Applicante { get; set; }
    }
}
