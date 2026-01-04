using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class TransactionCertification:BasicEntity
    {
        public int Id { get; set; }
        public int Idtransaction { get; set; }
        public string CertificateName { get; set; }//
        public string CertificatePath { get; set; }
        public string CertificateFilename { get; set; }
        public DateTime? Received { get; set; }
        public DateTime? BuyerSeen { get; set; }
        public bool? BuyerAccept { get; set; }
        public bool? BuyerRefuse { get; set; }
        public DateTime? BuyerResponse { get; set; }
        public bool? BuyerWeSolve { get; set; }
        public string BuyerWhyRejected { get; set; }
        public DateTime? SellerSeen { get; set; }
        public bool? SellerAccept { get; set; }
        public bool? SellerRefuse { get; set; }
        public DateTime? SellerResponse { get; set; }
        public bool? SellerWeSolve { get; set; }
        public string SellerWhyRejected { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public DateTime? ReceiptForecast { get; set; }
        public decimal Value { get; set; }
        public string EcartorioId { get; set; }
        public string Notary { get; set; }

        public string[] Errors { get; set; }
    }
}
