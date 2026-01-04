using System;

namespace Com.ByteAnalysis.IFacilita.GuideRequest.Application.Models
{
    public class GuideInput
    {
        public string ProtocolNumber { get; set; }

        public string Iptu { get; set; }

        public string PurchaserDocument { get; set; }

        public decimal Value { get; set; }

        public decimal BaseCalculation { get; set; }

        public decimal ValueTax { get; set; }

        public decimal ValueMora { get; set; }

        public decimal ValuePenalty { get; set; }

        public decimal TotalValue { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? AllowedPayment { get; set; }

        public string GuideNumber { get; set; }

        public string BilletBase64 { get; set; }
    }
}
