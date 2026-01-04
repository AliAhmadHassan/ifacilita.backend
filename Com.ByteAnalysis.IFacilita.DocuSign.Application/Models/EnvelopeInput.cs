using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class EnvelopeInput : BaseInput
    {
        public List<Dictionary<string, string>> Optionals { get; set; }

        public EnvelopeDocuSignInput EnvelopeDocuSign { get; set; }
    }
}
