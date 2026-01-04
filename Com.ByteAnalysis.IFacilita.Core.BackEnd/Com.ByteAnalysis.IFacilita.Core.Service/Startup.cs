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
			services.AddSingleton<Service.IAddressService, Service.Impl.AddressService>();
			services.AddSingleton<Service.IBankService, Service.Impl.BankService>();
			services.AddSingleton<Service.IBrokerService, Service.Impl.BrokerService>();
			services.AddSingleton<Service.ICityService, Service.Impl.CityService>();
			services.AddSingleton<Service.IDocumentService, Service.Impl.DocumentService>();
			services.AddSingleton<Service.IDocumentTypeService, Service.Impl.DocumentTypeService>();
			services.AddSingleton<Service.INotificationService, Service.Impl.NotificationService>();
			services.AddSingleton<Service.IPatrimonyService, Service.Impl.PatrimonyService>();
			services.AddSingleton<Service.IPatrimonyAcquirerTypeService, Service.Impl.PatrimonyAcquirerTypeService>();
			services.AddSingleton<Service.IPatrimonyDocumentService, Service.Impl.PatrimonyDocumentService>();
			services.AddSingleton<Service.IPatrimonyTransmitterTypeService, Service.Impl.PatrimonyTransmitterTypeService>();
			services.AddSingleton<Service.IPlatformBilletService, Service.Impl.PlatformBilletService>();
			services.AddSingleton<Service.IPlatformBilletBankDataService, Service.Impl.PlatformBilletBankDataService>();
			services.AddSingleton<Service.IPlatformSubWorkflowService, Service.Impl.PlatformSubWorkflowService>();
			services.AddSingleton<Service.IPlatformWorkflowService, Service.Impl.PlatformWorkflowService>();
			services.AddSingleton<Service.IRealEstateService, Service.Impl.RealEstateService>();
			services.AddSingleton<Service.IRealEstateBrokerService, Service.Impl.RealEstateBrokerService>();
			services.AddSingleton<Service.IRegistryService, Service.Impl.RegistryService>();
			services.AddSingleton<Service.ISendInviteService, Service.Impl.SendInviteService>();
			services.AddSingleton<Service.ISendInviteTextTypeService, Service.Impl.SendInviteTextTypeService>();
			services.AddSingleton<Service.ITransactionService, Service.Impl.TransactionService>();
			services.AddSingleton<Service.ITransactionDocumentService, Service.Impl.TransactionDocumentService>();
			services.AddSingleton<Service.IUserService, Service.Impl.UserService>();
			services.AddSingleton<Service.IUserBankDataService, Service.Impl.UserBankDataService>();
			services.AddSingleton<Service.IUserDocumentService, Service.Impl.UserDocumentService>();
			services.AddSingleton<Service.IUserProfileService, Service.Impl.UserProfileService>();
			services.AddSingleton<Service.IUserSpouseService, Service.Impl.UserSpouseService>();
			services.AddSingleton<Service.IUserSpouseTypeService, Service.Impl.UserSpouseTypeService>();
            repositoryStartup.ConfigureServices(services);
}
    }
}