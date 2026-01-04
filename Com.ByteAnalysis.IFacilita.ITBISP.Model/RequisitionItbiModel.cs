using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Model
{
    public class RequisitionItbiModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// NATUREZA DA TRANSAÇÃO
        /// </summary>
        public string Transaction { get; set; }

        /// <summary>
        /// CADASTRO DO IMÓVEL
        /// </summary>
        public string Iptu { get; set; }

        /// <summary>
        /// Comprador
        /// </summary>
        public IEnumerable<BuyerModel> Buyers { get; set; }

        /// <summary>
        /// Vendedor
        /// </summary>
        public IEnumerable<SellerModel> Sellers { get; set; }

        /// <summary>
        /// VALOR (OU PREÇO) TOTAL DA COMPRA E VENDA
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// TIPO FINANCIAMENTO
        /// </summary>
        public string FinancingType { get; set; }

        /// <summary>
        /// VALOR FINANCIADO
        /// </summary>
        public decimal? ValueFinancing { get; set; }

        /// <summary>
        /// ESTA SENDO TRANSMITIDA A TOTALIDADE DO IMÓVEL
        /// </summary>
        public bool Totality { get; set; }

        /// <summary>
        /// PROPORÇÃO TRANSMITIDA %
        /// </summary>
        public decimal? Proportion { get; set; }

        /// <summary>
        /// True = ESCRITURA PÚBLICA DE COMPRA E VENDA,
        /// False = INSTRUMENTO PARTICULAR (OU CONTRATO) JUNTO AO BANCO OU INSTITUIÇÃO FINANCEIRA
        /// </summary>
        public bool PublicScripture { get; set; }

        /// <summary>
        /// DATA DO INSTRUMENTO PARTICULAR (OU CONTRATO) JUNTO AO BANCO OU INSTITUIÇÃO FINANCEIR ou
        /// DATA DA ESCRITURA PÚBLICA
        /// </summary>
        public DateTime DateEvent { get; set; }

        /// <summary>
        /// CARTÓRIO DE NOTAS
        /// </summary>
        public string NotesOffice { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public string Uf { get; set; }

        /// <summary>
        /// Município
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Cartório
        /// </summary>
        public int Registry { get; set; }

        /// <summary>
        /// Matrícula
        /// </summary>
        public string Registration { get; set; }

        public bool Pending { get; set; }

        public bool Approved { get; set; }

        public CalculationModel Calculation { get; set; }

        public string UrlBillet { get; set; }

        /// <summary>
        /// http://ifacilita.com:5400/api/transaction/{id}/callback-itbi-sp-callback
        /// </summary>
        public string UrlCallback { get; set; }

        public int? TransactionId { get; set; }


        public string ResponseUrlCallback { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }

        public APIStatus Status { get; set; }
    }
}
