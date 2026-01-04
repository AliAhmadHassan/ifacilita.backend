using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.eCartorioSp.Entity.Dto
{
    public class OrderDto
    {
        public string Cpf { get; set; }

        public string Cnpj { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<OrderRegistryDto> Registry { get; set; }

        public PropertyDetailsDto PropertyDetails { get; set; }

        public List<DataSearchDto> DataSearch { get; set; }
    }
}
