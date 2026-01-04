namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class UserDocuSignResponse
    {
        public string UserId { get; set; }

        public string Uri { get; set; }

        public string ApiPassword { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string PermissionProfileId { get; set; }

        public string PermissionProfileName { get; set; }

        public string UserStatus { get; set; }

        public string CreatedDateTime { get; set; }

        public ErrorDetailsOutput ErrorDetails { get; set; }
    }
}
