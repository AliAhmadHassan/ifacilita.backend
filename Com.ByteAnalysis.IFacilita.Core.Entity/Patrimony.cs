using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.Core.Entity
{
    public class Patrimony: BasicEntity
    {
        public String MunicipalRegistration { get; set; }
        public String Registration { get; set; }
        public Int16? Bedrooms { get; set; }
        public Boolean? MaidsRoom { get; set; }
        public Int16? NumberOfCarSpaces { get; set; }
        public Boolean? ForeiroProperty { get; set; }
        public Int16? BathroomsExceptForMaids { get; set; }
        public Boolean? MaidBathroom { get; set; }
        public Boolean? Balcony { get; set; }
        public Int16? FloorPosition { get; set; }
        public Int32? IdAddress { get; set; }
        public Address Address { get; set; }
        public Int32? IdPatrimonyAcquirerType { get; set; }
        public Int32? IdPatrimonyTransmitterType { get; set; }

        public Boolean? Elevator { get; set; }

        public Boolean? RecreationArea { get; set; }

        public string IdItbiRobot { get; set; }

        public int StreetType { get; set; }

        public String SqlIptu { get; set; }

        public int NotaryNumber { get; set; }

        public bool Dependent { get; set; }

        public int Id { get; set; }

        #region ITBI SP

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
        public bool TotalityTransfer { get; set; }

        /// <summary>
        /// PROPORÇÃO TRANSMITIDA %
        /// </summary>
        public decimal? ProportionTransfer { get; set; }

        /// <summary>
        /// True = ESCRITURA PÚBLICA DE COMPRA E VENDA,
        /// False = INSTRUMENTO PARTICULAR (OU CONTRATO) JUNTO AO BANCO OU INSTITUIÇÃO FINANCEIRA
        /// </summary>
        public bool PublicScripture { get; set; }

        /// <summary>
        /// DATA DO INSTRUMENTO PARTICULAR (OU CONTRATO) JUNTO AO BANCO OU INSTITUIÇÃO FINANCEIR ou
        /// DATA DA ESCRITURA PÚBLICA
        /// </summary>
        public DateTime? DateEventScripture { get; set; }

        /// <summary>
        /// CARTÓRIO DE NOTAS
        /// </summary>
        public string ScriptureNotesOffice { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public string ScriptureUf { get; set; }

        /// <summary>
        /// Município
        /// </summary>
        public string ScriptureCity { get; set; }

        /// <summary>
        /// Declaração de quitação dos debitos de condominio
        /// </summary>
        public string CondominiumDeclarationDebts { get; set; }

        #endregion
    }
}