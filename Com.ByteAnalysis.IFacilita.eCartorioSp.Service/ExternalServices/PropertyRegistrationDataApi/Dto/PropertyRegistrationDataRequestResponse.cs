using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.PropertyRegistrationDataApi.Dto
{
    public class PropertyRegistrationDataRequestResponse
    {
        public String Id { get; set; }

        public String CpfCnPj { get; set; }

        public String Password { get; set; }

        public String Sql { get; set; }

        public String dateDoc { get; set; }

        public DateTime? Request { get; set; }

        public string UrlCallback { get; set; }

        public DateTime? Received { get; set; }

        public string UrlCertification { get; set; }

        public DateTime? Expiration { get; set; }

        public DateTime? CallbackResponse { get; set; }

        public ApiProcessStatus StatusProcess { get; set; }

        public DateTime? StatusModified { get; set; }

        public string s3patch { get; set; }

        public APIStatus Status { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }
    }
}
