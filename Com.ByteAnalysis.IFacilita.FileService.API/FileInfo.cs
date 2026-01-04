using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.FileService.API
{
    public class FileInfo
    {
        public string Base64encodedstring { get; set; }
        public string FileExtensionWithDot { get; set; }
        public string FileNameWithoutExtension { get; set; }
    }
}
