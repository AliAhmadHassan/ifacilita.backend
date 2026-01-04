using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.DefectsDefinedApi.Dto
{
    public class CertificateDefectsDefinedEntityRequestResponse
    {
        public string Id { get; set; }

        public int ModelCode { get; set; }

        public PersonType PersonType { get; set; }

        public string FullName { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public GenderType GenderType { get; set; }

        public string Email { get; set; }

        public bool Pending { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

        public DataOrderDto DataOrder { get; set; }

        public APIStatus Status { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }
    }

    public class DataOrderDto
    {
        public string NumberOrder { get; set; }

        public DateTime DateOrder { get; set; }

        public string UrlCertificate { get; set; }
    }
}
