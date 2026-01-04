namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class AccountManagementGranularOutput
    {
        public string CanManageUsers { get; set; }

        public string CanManageAdmins { get; set; }

        public string CanManageGroups { get; set; }

        public string CanManageSharing { get; set; }

        public string CanManageAccountSettings { get; set; }

        public string CanManageReporting { get; set; }

        public string CanManageAccountSecuritySettings { get; set; }

        public string CanManageSigningGroups { get; set; }

        public string CanViewUsers { get; set; }

        public CanManageUsersMetadataOutput CanManageUsersMetadata { get; set; }

        public CanManageAdminsMetadataOutput CanManageAdminsMetadata { get; set; }

        public CanManageGroupsMetadataOutput CanManageGroupsMetadata { get; set; }

        public CanManageSharingMetadataOutput CanManageSharingMetadata { get; set; }

        public CanManageAccountSettingsMetadataOutput CanManageAccountSettingsMetadata { get; set; }

        public CanManageReportingMetadataOutput CanManageReportingMetadata { get; set; }

        public CanManageAccountSecuritySettingsMetadataOut CanManageAccountSecuritySettingsMetadata { get; set; }

        public CanManageSigningGroupsMetadataOutput CanManageSigningGroupsMetadata { get; set; }

        public CanViewUsersMetadataOutput CanViewUsersMetadata { get; set; }
    }
}