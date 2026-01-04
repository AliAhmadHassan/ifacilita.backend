using Newtonsoft.Json;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class AccountManagementGranular
    {
        [JsonProperty("canManageUsers")]
        public string CanManageUsers { get; set; }

        [JsonProperty("canManageAdmins")]
        public string CanManageAdmins { get; set; }

        [JsonProperty("canManageGroups")]
        public string CanManageGroups { get; set; }

        [JsonProperty("canManageSharing")]
        public string CanManageSharing { get; set; }

        [JsonProperty("canManageAccountSettings")]
        public string CanManageAccountSettings { get; set; }

        [JsonProperty("canManageReporting")]
        public string CanManageReporting { get; set; }

        [JsonProperty("canManageAccountSecuritySettings")]
        public string CanManageAccountSecuritySettings { get; set; }

        [JsonProperty("canManageSigningGroups")]
        public string CanManageSigningGroups { get; set; }

        [JsonProperty("canViewUsers")]
        public string CanViewUsers { get; set; }

        [JsonProperty("canManageUsersMetadata")]
        public CanManageUsersMetadata CanManageUsersMetadata { get; set; }

        [JsonProperty("canManageAdminsMetadata")]
        public CanManageAdminsMetadata CanManageAdminsMetadata { get; set; }

        [JsonProperty("canManageGroupsMetadata")]
        public CanManageGroupsMetadata CanManageGroupsMetadata { get; set; }

        [JsonProperty("canManageSharingMetadata")]
        public CanManageSharingMetadata CanManageSharingMetadata { get; set; }

        [JsonProperty("canManageAccountSettingsMetadata")]
        public CanManageAccountSettingsMetadata CanManageAccountSettingsMetadata { get; set; }

        [JsonProperty("canManageReportingMetadata")]
        public CanManageReportingMetadata CanManageReportingMetadata { get; set; }

        [JsonProperty("canManageAccountSecuritySettingsMetadata")]
        public CanManageAccountSecuritySettingsMetadata CanManageAccountSecuritySettingsMetadata { get; set; }

        [JsonProperty("canManageSigningGroupsMetadata")]
        public CanManageSigningGroupsMetadata CanManageSigningGroupsMetadata { get; set; }

        [JsonProperty("canViewUsersMetadata")]
        public CanViewUsersMetadata CanViewUsersMetadata { get; set; }
    }
}
