namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models
{
    public class DataActSearchInput
    {
        public string NomeBusca { get; set; }

        public string NomePai { get; set; }

        public string NomeMae { get; set; }

        public string DataNascimento { get; set; }

        public string CpfCnpj { get; set; }
    }

    public class DataActSearchInputDto
    {
        public string NameSearch { get; set; }

        public string FatherName { get; set; }

        public string MatherName { get; set; }

        public string BirthDate { get; set; }

        public string CpfCnpj { get; set; }
    }
}
