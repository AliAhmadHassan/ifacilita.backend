using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class UserInput : BaseInput
    {
        public List<OptionalsInput> Optionals { get; set; }

        public UserDocuSignInput Users { get; set; }
    }
}
