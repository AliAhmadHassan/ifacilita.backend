using Newtonsoft.Json;
using System.Collections.Generic;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class UserResponse
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userType")]
        public string UserType { get; set; }

        [JsonProperty("isAdmin")]
        public string IsAdmin { get; set; }

        [JsonProperty("userStatus")]
        public string UserStatus { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("suffixName")]
        public string SuffixName { get; set; }

        [JsonProperty("permissionProfileId")]
        public string PermissionProfileId { get; set; }

        [JsonProperty("permissionProfileName")]
        public string PermissionProfileName { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("subscribe")]
        public string Subscribe { get; set; }

        [JsonProperty("userSettings")]
        public List<UserSetting> UserSettings { get; set; }

        [JsonProperty("accountManagementGranular")]
        public AccountManagementGranular AccountManagementGranular { get; set; }

        [JsonProperty("sendActivationOnInvalidLogin")]
        public string SendActivationOnInvalidLogin { get; set; }

        [JsonProperty("activationAccessCode")]
        public string ActivationAccessCode { get; set; }

        [JsonProperty("enableConnectForUser")]
        public string EnableConnectForUser { get; set; }

        [JsonProperty("forgottenPasswordInfo")]
        public ForgottenPasswordInfo ForgottenPasswordInfo { get; set; }

        [JsonProperty("groupList")]
        public List<GroupList> GroupList { get; set; }

        [JsonProperty("workAddress")]
        public WorkAddress WorkAddress { get; set; }

        [JsonProperty("homeAddress")]
        public HomeAddress HomeAddress { get; set; }

        [JsonProperty("loginStatus")]
        public string LoginStatus { get; set; }

        [JsonProperty("passwordExpiration")]
        public string PasswordExpiration { get; set; }

        [JsonProperty("lastLogin")]
        public string LastLogin { get; set; }

        [JsonProperty("sendActivationEmail")]
        public string SendActivationEmail { get; set; }

        [JsonProperty("customSettings")]
        public List<CustomSetting> CustomSettings { get; set; }

        [JsonProperty("profileImageUri")]
        public string ProfileImageUri { get; set; }

        [JsonProperty("userProfileLastModifiedDate")]
        public string UserProfileLastModifiedDate { get; set; }

        [JsonProperty("signatureImageUri")]
        public string SignatureImageUri { get; set; }

        [JsonProperty("initialsImageUri")]
        public string InitialsImageUri { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }
    }
}
