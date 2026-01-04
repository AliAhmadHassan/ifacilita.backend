using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using Com.ByteAnalysis.IFacilita.eCartorioSp.Entity;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Service.ExternalServices.TaxDebtsApi.Dto
{
    public class TaxDebtsRequestResponse
    {
        public String Id { get; set; }

        public string CpfCnpj { get; set; }

        public String Nome { get; set; }

        public bool PessoaFisica { get; set; }

        public int IdUserIfacilita { get; set; }

        public String IdDocS3 { get; set; }

        public DateTime DateInsert { get; set; }

        public ApiProcessStatus StatusProcess { get; set; }

        public DateTime StatusModified { get; set; }

        public string UrlCallback { get; set; }

        public APIStatus Status { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }
    }
}
