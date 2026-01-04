using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Model
{
    [Obsolete("Essa classe está desatualizada")]
    public class Requisition
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }
        
        //Dados Imovel

        public String CadastroImovel { get; set; }

        public String CepImovel { get; set; }

        public String LogradouroImovel { get; set; }

        public int NumeroImovel { get; set; }

        public String ComplementoImovel { get; set; }

        public String MunicipioImovel { get; set; }

        //Dados Comprador
        public String NomeComprador { get; set; }

        public String CpfCnpj { get; set; }

        public String CepComprador { get; set; }

        public String LogradouroComprador { get; set; }

        public int NumeroComprador { get; set; }

        public String ComplementoComprador { get; set; }

        public String MunicipioComprador { get; set; }

        public String TelefoneComprador { get; set; }

        public String EmailVendedor { get; set; }

        //Dados Vendedor
        public String NomeVendedor { get; set; }

        public String CpfCnpjVendedor { get; set; }

        //Dados Venda
        public double ValorTrasacao { get; set; }

        public double ValorFinanciado { get; set; }

        public String DataTransacao { get; set; }

        public String TipoFinanciamento { get; set; }

        public Boolean IsParticular { get; set; } = true;

        public Boolean IsTotalImovel { get; set; } = true;

        public int NumCartorioImovel { get; set; }

        //Dados RPA
        public DateTime Request { get; set; }

        public string UrlCallback { get; set; }

        public DateTime Received { get; set; }

        public string UrlCertification { get; set; }

        public DateTime Expiration { get; set; }

        public DateTime CallbackResponse { get; set; }

        public Status StatusProcessing { get; set; }

        public DateTime StatusModified { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }

        public APIStatus Status { get; set; }
    }
}
