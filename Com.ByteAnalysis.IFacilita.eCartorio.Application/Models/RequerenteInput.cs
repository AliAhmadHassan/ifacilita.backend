using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models
{
    public class RequerenteInput: BaseInput
    {
        /// <summary>
        /// CPF do requerente
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// CNPJ do requerente
        /// </summary>
        public string Cnpj { get; set; }

        /// <summary>
        /// Nome do Requerente
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Email do requerente
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Url que o sistema deverá responder quando finalizar o pedido por completo
        /// </summary>
        public string UrlCallback { get; set; }

        public IEnumerable<RequerenteActRegistryInput> ActRegistry { get; set; }

        /// <summary>
        /// Dados do Imóvel
        /// </summary>
        public PropertyDetailsInput PropertyDetails { get; set; }

        /// <summary>
        /// Dados Buscar
        /// </summary>
        public IEnumerable<DataActSearchInput> DataActSearch { get; set; }
    }

    public class ApplicantInputDto : BaseInput
    {
        public string Cpf { get; set; }

        public string Cnpj { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string UrlCallback { get; set; }

        public IEnumerable<RequerenteActRegistryInput> ActRegistry { get; set; }

        public PropertyDetailsInputDto PropertyDetails { get; set; }

        public IEnumerable<DataActSearchInputDto> DataActSearch { get; set; }
    }
}
