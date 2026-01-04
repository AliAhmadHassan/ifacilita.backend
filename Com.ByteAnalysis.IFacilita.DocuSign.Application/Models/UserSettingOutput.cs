using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Application.Models
{
    public class UserSettingOutput
    {
        public bool CanManageAccount { get; set; }

        public AccountManagementGranularOutput AccountManagementGranular { get; set; }

        public bool CanSendEnvelope { get; set; }

        public bool CanSendApiRequests { get; set; }

        public bool ApiAccountWideAccess { get; set; }

        public bool EnableVaulting { get; set; }

        public string VaultingMode { get; set; }

        public bool EnableTransactionPoint { get; set; }

        public bool EnableSequentialSigningApi { get; set; }

        public bool EnableSequentialSigningUi { get; set; }

        public bool EnableDsPro { get; set; }

        public string PowerFormMode { get; set; }

        public bool AllowPowerFormsAdminToAccessAllPowerFormEnvelope { get; set; }

        public string CanEditSharedAddressbook { get; set; }

        public string ManageClickwrapsMode { get; set; }

        public bool EnableSignOnPaperOverride { get; set; }

        public bool EnableSignerAttachments { get; set; }

        public bool AllowSendOnBehalfOf { get; set; }

        public string CanManageTemplates { get; set; }

        public bool AllowEnvelopeTransferTo { get; set; }

        public bool AllowRecipientLanguageSelection { get; set; }

        public bool ApiCanExportAc { get; set; }

        public bool BulkSend { get; set; }

        public bool CanChargeAccount { get; set; }

        public bool CanManageDistributor { get; set; }

        public bool CanSignEnvelope { get; set; }

        public bool NewSendUi { get; set; }

        public bool RecipientViewedNotification { get; set; }

        public bool TemplateActiveCreation { get; set; }

        public bool TemplateApplyNotify { get; set; }

        public bool TemplateAutoMatching { get; set; }

        public long TemplateMatchingSensitivity { get; set; }

        public bool TemplatePageLevelMatching { get; set; }

        public string TransactionPointSiteNameUrl { get; set; }

        public string TransactionPointUserName { get; set; }

        public string TimezoneOffset { get; set; }

        public string TimezoneMask { get; set; }

        public bool TimezoneDst { get; set; }

        public Guid ModifiedBy { get; set; }

        public string ModifiedPage { get; set; }

        public string ModifiedDate { get; set; }

        public bool AdminOnly { get; set; }

        public string SelfSignedRecipientEmailDocument { get; set; }

        public SignerEmailNotificationsOutput SignerEmailNotifications { get; set; }

        public SenderEmailNotificationsOutput SenderEmailNotifications { get; set; }

        public LocalePolicyOutput LocalePolicy { get; set; }

        public string Locale { get; set; }

        public bool CanLockEnvelopes { get; set; }

        public bool CanUseScratchpad { get; set; }

        public bool CanCreateWorkspaces { get; set; }

        public bool IsWorkspaceParticipant { get; set; }

        public bool AllowEmailChange { get; set; }

        public bool AllowPasswordChange { get; set; }

        public string FederatedStatus { get; set; }

        public bool AllowSupplementalDocuments { get; set; }

        public bool SupplementalDocumentsMustView { get; set; }

        public bool SupplementalDocumentsMustAccept { get; set; }

        public bool SupplementalDocumentsMustRead { get; set; }

        public bool CanManageOrganization { get; set; }

        public bool ExpressSendOnly { get; set; }

        public bool SupplementalDocumentIncludeInDownload { get; set; }

        public bool DisableDocumentUpload { get; set; }

        public bool DisableOtherActions { get; set; }

        public bool UseAccountServerForPasswordChange { get; set; }

        public bool IsCommentsParticipant { get; set; }

        public bool UseAccountServerForEmailChange { get; set; }

        public bool AllowEsealRecipients { get; set; }

        public object[] SealIdentifiers { get; set; }

        public bool AgreedToComments { get; set; }

        public bool CanUseSmartContracts { get; set; }
    }
}