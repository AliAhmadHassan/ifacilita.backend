
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Core.Entity.PaymentGateway
{
    public class InvoiceDto
    {
        public string ReturnUrl { get; set; }

        public string NotificationUrl { get; set; }

        public string ExternalId { get; set; }

        public int MaxInstallments { get; set; }

        public PaymentCompany PaymentCompany { get; set; }

        public string PayableWith { get; set; }

        public string ResponsePaymentCompany { get; set; }

        public DateTime DueDate { get; set; }

        public long ClientId { get; set; }

        public virtual IEnumerable<InvoiceProductDto> Itens { get; set; }

        public string UrlBillet { get; set; }
    }
}
