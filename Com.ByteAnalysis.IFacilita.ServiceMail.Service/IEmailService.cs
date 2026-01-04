using Com.ByteAnalysis.IFacilita.Core.Service.EmailServices.Dto;
using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.ServiceMail.Service
{
    public interface IEmailService
    {
        Tuple<bool, string> Send(EmailMessage emailMessage, string[] attachment);

        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
