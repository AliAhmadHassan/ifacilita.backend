using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.Core.Service
{
    public class Startup
    {
        private Repository.Startup repositoryStartup;

        public Startup()
        {
            repositoryStartup = new Repository.Startup();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAddressService, Service.Impl.AddressService>();
            services.AddSingleton<IBankService, Service.Impl.BankService>();
            services.AddSingleton<IBrokerService, Service.Impl.BrokerService>();
            services.AddSingleton<ICityService, Service.Impl.CityService>();
            services.AddSingleton<INotificationService, Service.Impl.NotificationService>();
            services.AddSingleton<IPatrimonyService, Service.Impl.PatrimonyService>();
            services.AddSingleton<IPatrimonyAcquirerTypeService, Service.Impl.PatrimonyAcquirerTypeService>();
            services.AddSingleton<IPatrimonyTransmitterTypeService, Service.Impl.PatrimonyTransmitterTypeService>();
            services.AddSingleton<IPlatformBilletService, Service.Impl.PlatformBilletService>();
            services.AddSingleton<IPlatformBilletBankDataService, Service.Impl.PlatformBilletBankDataService>();
            services.AddSingleton<IPlatformSubWorkflowService, Service.Impl.PlatformSubWorkflowService>();
            services.AddSingleton<IPlatformWorkflowService, Service.Impl.PlatformWorkflowService>();
            services.AddSingleton<IRealEstateService, Service.Impl.RealEstateService>();
            services.AddSingleton<IRealEstateBrokerService, Service.Impl.RealEstateBrokerService>();
            services.AddSingleton<IRegistryService, Service.Impl.RegistryService>();
            services.AddSingleton<ISendInviteService, Service.Impl.SendInviteService>();
            services.AddSingleton<ISendInviteTextTypeService, Service.Impl.SendInviteTextTypeService>();
            services.AddSingleton<ITransactionService, Service.Impl.TransactionService>();
            services.AddSingleton<IUserService, Service.Impl.UserService>();
            services.AddSingleton<IUserBankDataService, Service.Impl.UserBankDataService>();
            services.AddSingleton<IUserDocumentService, Service.Impl.UserDocumentService>();
            services.AddSingleton<IUserProfileService, Service.Impl.UserProfileService>();
            services.AddSingleton<IUserSpouseService, Service.Impl.UserSpouseService>();
            services.AddSingleton<IUserSpouseTypeService, Service.Impl.UserSpouseTypeService>();
            services.AddSingleton<ITransactionFlowService, Service.Impl.TransactionFlowService>();
            services.AddSingleton<ITransactionPaymentFormService, Service.Impl.TransactionPaymentFormService>();
            services.AddSingleton<ITransactionCertificationService, Service.Impl.TransactionCertificationService>();
            services.AddSingleton<IPushNotificationService, Service.Impl.PushNotificationService>();
            services.AddSingleton<IHistoricService, Service.Impl.HistoricService>();
            services.AddSingleton<ITransactionHiredService, Service.Impl.TransactionHiredService>();
            services.AddSingleton<ITransactionRGIService, Service.Impl.TransactionRGIService>();
            services.AddSingleton<IUserNaturgyService, Service.Impl.UserNaturgyService>();
            services.AddSingleton<ILandpageService, Service.Impl.LandpageService>();
            services.AddSingleton<ExternalServices.PaymentsGateway.IClientApi, ExternalServices.PaymentsGateway.Impl.ClientApi>();
            services.AddSingleton<ExternalServices.PaymentsGateway.IClientApiPaymentsGateway, ExternalServices.PaymentsGateway.Impl.ClientApiPaymentsGateway>();

            repositoryStartup.ConfigureServices(services);
        }
    }
}