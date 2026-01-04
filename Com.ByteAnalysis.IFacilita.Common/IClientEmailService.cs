using System;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.Common
{
    public interface IClientEmailService
    {
        bool SendMail(IList<Tuple<string, string>> emailTo, string subject, string messageInHtml, string[] pathAttachments, List<object> otherFields, string messageType= "Wellcome");
    }
}
