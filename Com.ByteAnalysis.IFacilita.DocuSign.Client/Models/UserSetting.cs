using Newtonsoft.Json;
using System;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Client.Models
{
    public class UserSetting
    {
        [JsonProperty("canManageAccount")]
        public bool CanManageAccount { get; set; }

        [JsonProperty("accountManagementGranular")]
        public AccountManagementGranular AccountManagementGranular { get; set; }

        [JsonProperty("canSendEnvelope")]
        public bool CanSendEnvelope { get; set; }

        [JsonProperty("canSendAPIRequests")]
        public bool CanSendApiRequests { get; set; }

        [JsonProperty("apiAccountWideAccess")]
        public bool ApiAccountWideAccess { get; set; }

        [JsonProperty("enableVaulting")]
        public bool EnableVaulting { get; set; }

        [JsonProperty("vaultingMode")]
        public string VaultingMode { get; set; }

        [JsonProperty("enableTransactionPoint")]
        public bool EnableTransactionPoint { get; set; }

        [JsonProperty("enableSequentialSigningAPI")]
        public bool EnableSequentialSigningApi { get; set; }

        [JsonProperty("enableSequentialSigningUI")]
        public bool EnableSequentialSigningUi { get; set; }

        [JsonProperty("enableDSPro")]
        public bool EnableDsPro { get; set; }

        [JsonProperty("powerFormMode")]
        public string PowerFormMode { get; set; }

        [JsonProperty("allowPowerFormsAdminToAccessAllPowerFormEnvelope")]
        public bool AllowPowerFormsAdminToAccessAllPowerFormEnvelope { get; set; }

        [JsonProperty("canEditSharedAddressbook")]
        public string CanEditSharedAddressbook { get; set; }

        [JsonProperty("manageClickwrapsMode")]
        public string ManageClickwrapsMode { get; set; }

        [JsonProperty("enableSignOnPaperOverride")]
        public bool EnableSignOnPaperOverride { get; set; }

        [JsonProperty("enableSignerAttachments")]
        public bool EnableSignerAttachments { get; set; }

        [JsonProperty("allowSendOnBehalfOf")]
        public bool AllowSendOnBehalfOf { get; set; }

        [JsonProperty("canManageTemplates")]
        public string CanManageTemplates { get; set; }

        [JsonProperty("allowEnvelopeTransferTo")]
        public bool AllowEnvelopeTransferTo { get; set; }

        [JsonProperty("allowRecipientLanguageSelection")]
        public bool AllowRecipientLanguageSelection { get; set; }

        [JsonProperty("apiCanExportAC")]
        public bool ApiCanExportAc { get; set; }

        [JsonProperty("bulkSend")]
        public bool BulkSend { get; set; }

        [JsonProperty("canChargeAccount")]
        public bool CanChargeAccount { get; set; }

        [JsonProperty("canManageDistributor")]
        public bool CanManageDistributor { get; set; }

        [JsonProperty("canSignEnvelope")]
        public bool CanSignEnvelope { get; set; }

        [JsonProperty("newSendUI")]
        public bool NewSendUi { get; set; }

        [JsonProperty("recipientViewedNotification")]
        public bool RecipientViewedNotification { get; set; }

        [JsonProperty("templateActiveCreation")]
        public bool TemplateActiveCreation { get; set; }

        [JsonProperty("templateApplyNotify")]
        public bool TemplateApplyNotify { get; set; }

        [JsonProperty("templateAutoMatching")]
        public bool TemplateAutoMatching { get; set; }

        [JsonProperty("templateMatchingSensitivity")]
        public long TemplateMatchingSensitivity { get; set; }

        [JsonProperty("templatePageLevelMatching")]
        public bool TemplatePageLevelMatching { get; set; }

        [JsonProperty("transactionPointSiteNameURL")]
        public string TransactionPointSiteNameUrl { get; set; }

        [JsonProperty("transactionPointUserName")]
        public string TransactionPointUserName { get; set; }

        [JsonProperty("timezoneOffset")]
        public string TimezoneOffset { get; set; }

        [JsonProperty("timezoneMask")]
        public string TimezoneMask { get; set; }

        [JsonProperty("timezoneDST")]
        public bool TimezoneDst { get; set; }

        [JsonProperty("modifiedBy")]
        public Guid ModifiedBy { get; set; }

        [JsonProperty("modifiedPage")]
        public string ModifiedPage { get; set; }

        [JsonProperty("modifiedDate")]
        public string ModifiedDate { get; set; }

        [JsonProperty("adminOnly")]
        public bool AdminOnly { get; set; }

        [JsonProperty("selfSignedRecipientEmailDocument")]
        public string SelfSignedRecipientEmailDocument { get; set; }

        [JsonProperty("signerEmailNotifications")]
        public SignerEmailNotifications SignerEmailNotifications { get; set; }

        [JsonProperty("senderEmailNotifications")]
        public SenderEmailNotifications SenderEmailNotifications { get; set; }

        [JsonProperty("localePolicy")]
        public LocalePolicy LocalePolicy { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("canLockEnvelopes")]
        public bool CanLockEnvelopes { get; set; }

        [JsonProperty("canUseScratchpad")]
        public bool CanUseScratchpad { get; set; }

        [JsonProperty("canCreateWorkspaces")]
        public bool CanCreateWorkspaces { get; set; }

        [JsonProperty("isWorkspaceParticipant")]
        public bool IsWorkspaceParticipant { get; set; }

        [JsonProperty("allowEmailChange")]
        public bool AllowEmailChange { get; set; }

        [JsonProperty("allowPasswordChange")]
        public bool AllowPasswordChange { get; set; }

        [JsonProperty("federatedStatus")]
        public string FederatedStatus { get; set; }

        [JsonProperty("allowSupplementalDocuments")]
        public bool AllowSupplementalDocuments { get; set; }

        [JsonProperty("supplementalDocumentsMustView")]
        public bool SupplementalDocumentsMustView { get; set; }

        [JsonProperty("supplementalDocumentsMustAccept")]
        public bool SupplementalDocumentsMustAccept { get; set; }

        [JsonProperty("supplementalDocumentsMustRead")]
        public bool SupplementalDocumentsMustRead { get; set; }

        [JsonProperty("canManageOrganization")]
        public bool CanManageOrganization { get; set; }

        [JsonProperty("expressSendOnly")]
        public bool ExpressSendOnly { get; set; }

        [JsonProperty("supplementalDocumentIncludeInDownload")]
        public bool SupplementalDocumentIncludeInDownload { get; set; }

        [JsonProperty("disableDocumentUpload")]
        public bool DisableDocumentUpload { get; set; }

        [JsonProperty("disableOtherActions")]
        public bool DisableOtherActions { get; set; }

        [JsonProperty("useAccountServerForPasswordChange")]
        public bool UseAccountServerForPasswordChange { get; set; }

        [JsonProperty("isCommentsParticipant")]
        public bool IsCommentsParticipant { get; set; }

        [JsonProperty("useAccountServerForEmailChange")]
        public bool UseAccountServerForEmailChange { get; set; }

        [JsonProperty("allowEsealRecipients")]
        public bool AllowEsealRecipients { get; set; }

        [JsonProperty("sealIdentifiers")]
        public object[] SealIdentifiers { get; set; }

        [JsonProperty("agreedToComments")]
        public bool AgreedToComments { get; set; }

        [JsonProperty("canUseSmartContracts")]
        public bool CanUseSmartContracts { get; set; }
    }
}
