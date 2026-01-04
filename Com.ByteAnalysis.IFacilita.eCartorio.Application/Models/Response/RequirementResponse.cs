using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response
{
    public class RequirementResponse
    {
        public int IdExigencia { get; set; }

        public string Status { get; set; }

        public DateTime DataRegistro { get; set; }

        public DateTime DataPrazo { get; set; }

        public DateTime DataCumprimento { get; set; }

        public decimal? ValorExigencia { get; set; }

        public DateTime DataLiberacao { get; set; }

        public string TipoExigencia { get; set; }

        public IEnumerable<MessageResponse> Mensagens { get; set; }
    }

    public class RequirementDto
    {
        public int IdRequeriment { get; set; }

        public string Status { get; set; }

        public DateTime RegistryDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime DateCompliance { get; set; }

        public decimal? ValueRequirement { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string TypeRequirement { get; set; }

        public IEnumerable<MessageDto> Messages { get; set; }
    }
}
