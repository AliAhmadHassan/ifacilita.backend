using Com.ByteAnalysis.IFacilita.ServiceMail.Service.Dto;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Core.Service.EmailServices.Dto
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        }

        public List<EmailAddress> ToAddresses { get; set; }
        public List<EmailAddress> FromAddresses { get; set; }

        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
