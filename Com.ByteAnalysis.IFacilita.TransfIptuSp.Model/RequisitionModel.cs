using Com.ByteAnalysis.IFacilita.Common.Enumerable;
using Com.ByteAnalysis.IFacilita.Common.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.TransfIptuSp.Model
{
    public class RequisitionModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        /// <summary>
        /// Iptu ou SQL do imóvel
        /// </summary>
        public string IptuSql { get; set; }

        /// <summary>
        /// Atualização feita pelo COMPRADOR ou Vendedor
        /// </summary>
        public bool BuyerUpdate { get; set; }

        /// <summary>
        /// Nome do contribuinte:
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pessoa Física?
        /// </summary>
        public bool PhysicalPerson { get; set; }


        /// <summary>
        /// CPF/CNPJ
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Tipo de documento de propriedade:
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// Número da Matrícula:
        /// </summary>
        public string Registration { get; set; }

        /// <summary>
        /// O endereço de ENTREGA da notificação do IPTU é igual ao endereço do  imóvel?
        /// </summary>
        public bool IptuInLoco { get; set; }

        /// <summary>
        /// DATA DE PAGAMENTO DO IPTU
        /// </summary>
        public int PayDay { get; set; }

        /// <summary>
        /// Número do Cartório:
        /// </summary>
        public string Registry { get; set; }

        /// <summary>
        /// DATA DA AQUISIÇÃO DO IMÓVEL(data do registro na matricula,data do contrato ou data da escritura):
        /// </summary>
        public DateTime DateAcquisition { get; set; }

        /// <summary>
        ///O endereço de ENTREGA da notificação do IPTU é igual ao endereço do  imóvel?
        /// </summary>
        public bool AddressDeliveryIptuEquals { get; set; }

        public string Cep { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Bairro
        /// </summary>
        public string Neighborhood { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string Uf { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Ddd { get; set; }

        public string Phone { get; set; }

        public IEnumerable<OtherOwner> OtherOwners { get; set; }

        /// <summary>
        /// Deseja que a notificação do IPTU seja entregue em qual endereço ?
        /// </summary>
        public bool AddressDeliveryIptuDeclarant { get; set; }

        public string UrlRequisition { get; set; }

        public string UrlCallback { get; set; }

        public string UrlCallbackResponse { get; set; }

        public bool Pending { get; set; }

        public IEnumerable<GlobalError> Errors { get; set; }

        public APIStatus Status { get; set; }
    }

    public class OtherOwner
    {
        public string Document { get; set; }

        public string Name { get; set; }
    }
}
