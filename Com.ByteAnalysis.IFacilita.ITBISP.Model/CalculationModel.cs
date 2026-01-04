using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.ITBISP.Model
{
    public class CalculationModel
    {
        /// <summary>
        /// VALOR VENAL DE REFERÊNCIA
        /// </summary>
        public decimal ValueRef { get; set; }

        /// <summary>
        /// BASE DE CÁLCULO ADOTADA
        /// </summary>
        public decimal CalculationBase { get; set; }

        /// <summary>
        /// IMPOSTO
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// DATA DE VENCIMENTO
        /// </summary>
        public DateTime DateDue { get; set; }

        /// <summary>
        /// MULTA
        /// </summary>
        public decimal Fine { get; set; }

        /// <summary>
        /// Atualização Monetária
        /// </summary>
        public decimal Correction { get; set; }

        /// <summary>
        /// Juros
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public decimal Total { get; set; }

    }
}
