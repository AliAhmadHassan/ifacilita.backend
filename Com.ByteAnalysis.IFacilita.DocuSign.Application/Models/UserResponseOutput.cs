using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class UserResponseOutput
    {
        public string UserName { get; set; }

        public string UserId { get; set; }

        public string UserType { get; set; }

        public string IsAdmin { get; set; }

        public string UserStatus { get; set; }

        public string Uri { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Title { get; set; }

        public string CreatedDateTime { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string SuffixName { get; set; }

        public string PermissionProfileId { get; set; }

        public string PermissionProfileName { get; set; }

        public string CountryCode { get; set; }

        public string Subscribe { get; set; }

        //public UserSettingOutput[] UserSettings { get; set; }

        public AccountManagementGranularOutput AccountManagementGranular { get; set; }

        public string SendActivationOnInvalidLogin { get; set; }

        public string ActivationAccessCode { get; set; }

        public string EnableConnectForUser { get; set; }

        public ForgottenPasswordInfoOutput ForgottenPasswordInfo { get; set; }

        public List<GroupListOutput> GroupList { get; set; }

        public WorkAddressOutput WorkAddress { get; set; }

        public HomeAddressOutput HomeAddress { get; set; }

        public string LoginStatus { get; set; }

        public string PasswordExpiration { get; set; }

        public string LastLogin { get; set; }

        public string SendActivationEmail { get; set; }

        public List<CustomSettingOutput> CustomSettings { get; set; }

        public string ProfileImageUri { get; set; }

        public string UserProfileLastModifiedDate { get; set; }

        public string SignatureImageUri { get; set; }

        public string InitialsImageUri { get; set; }

        public string JobTitle { get; set; }
    }
}
