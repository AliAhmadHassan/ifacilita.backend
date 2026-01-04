using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Transaction: BasicEntity
    {
        public Int32 Id { get; set; }
        public DateTime? Date { get; set; }
        public Int32? IdUser { get; set; }
        public Int32? Seller { get; set; }
        public String PatrimonyMunicipalRegistration { get; set; }
        public Int32? IdPatrimonyAcquirerType { get; set; }
        public Int32? IdPatrimonyTransmitterType { get; set; }
        public Int32? IdRegistry { get; set; }
        public User User { get; set; }
        public User User_Seller { get; set; }
        public Patrimony Patrimony { get; set; }
        public PatrimonyAcquirerType PatrimonyAcquirerType { get; set; }
        public PatrimonyTransmitterType PatrimonyTransmitterType { get; set; }
        public Registry Registry { get; set; }
        public decimal? Signal { get; set; }
        public string PromiseVoucher { get; set; }
        public string PromiseVoucher_FileName { get; set; }
        public string CertificateVoucher { get; set; }
        public string CertificateVoucher_FileName { get; set; }

        public string ItbiVoucher { get; set; }

        public string ItbiVoucher_FileName { get; set; }

        public string IdDraft { get; set; }
        public string KeyDelivery { get; set; }
        public string RegistryToken { get; set; }
        public string ContractToken { get; set; }
        public int IdPatrimony { get; set; }
    }
}