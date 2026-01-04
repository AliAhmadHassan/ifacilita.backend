using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.SearchProtestApi.Dto
{
    public class CertificateSearchProtestResponse
    {
        public string Id { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        public Coverage Coverage { get; set; }

        public Expedition Expedition { get; set; }

        public PersonType PersonType { get; set; }

        public int JuridicalDistrictCode { get; set; }

        public bool AllRegistry { get; set; }

        public string FullName { get; set; }

        public string DocumentPrincipal { get; set; }

        public DocumentType DocumentType { get; set; }

        public string DocumentComplementary { get; set; }

        public string UrlBillet { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

        public bool Pending { get; set; }

        public bool Approved { get; set; }

        public bool HasProtest { get; set; }

        public string OrderNumber { get; set; }

        /// <summary>
        /// Os status permitidos para esse campo são: Unprocessed, Waiting, Downloaded
        /// </summary>
        public string StatusDownloadCertificates { get; set; }

        public IList<Certificate> Certificates { get; set; }

        public APIStatus Status { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }
    }

    public class Certificate
    {
        public string Registry { get; set; }

        public string UrlCertificatePdf { get; set; }

        public string UtlCertificateP7s { get; set; }
    }
}
