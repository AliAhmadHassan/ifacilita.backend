using System.ComponentModel.DataAnnotations;

namespace Com.ByteAnalysis.IFacilita.Core.Entity.Dto.Patrimony
{
    public class UploadDeclarationDto
    {
        [Required]
        public int IdPatrimony { get; set; }
        
        [Required]
        public string FileName { get; set; }

        [Required]
        public string DeclarationBase64 { get; set; }
    }
}
