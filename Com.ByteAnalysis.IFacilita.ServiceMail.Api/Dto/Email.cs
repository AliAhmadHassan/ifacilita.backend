namespace Com.ByteAnalysis.IFacilita.ServiceMail.Api.Dto
{
    public class Email
    {
        public EmailAddressApi[] From { get; set; }

        public EmailAddressApi[] To { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string MessageType { get; set; }

        public OtherFields[] OtherFields { get; set; }

        public EmailAttachmentApi[] Attachments { get; set; }

    }

    public class EmailAttachmentApi
    {
        public string Name { get; set; }

        public string Extension { get; set; }

        public string FileBase64 { get; set; }
    }

    public class EmailAddressApi
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }

    public class OtherFields
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
