namespace Com.ByteAnalysis.IFacilita.eCartorio.Application.Models.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }
    }

    public class CategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
