using Dapper.FluentMap;
using Microsoft.Extensions.DependencyInjection;

namespace Com.ByteAnalysis.IFacilita.Core.Repository
{
    public class Startup
    {
        public Startup()
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new Repository.DataMapper.AddressMap());
                config.AddMap(new Repository.DataMapper.BankMap());
                config.AddMap(new Repository.DataMapper.BrokerMap());
                config.AddMap(new Repository.DataMapper.CityMap());
                config.AddMap(new Repository.DataMapper.DocumentMap());
                config.AddMap(new Repository.DataMapper.DocumentTypeMap());
                config.AddMap(new Repository.DataMapper.NotificationMap());
                config.AddMap(new Repository.DataMapper.PatrimonyMap());
                config.AddMap(new Repository.DataMapper.PatrimonyAcquirerTypeMap());
                config.AddMap(new Repository.DataMapper.PatrimonyDocumentMap());
                config.AddMap(new Repository.DataMapper.PatrimonyTransmitterTypeMap());
                config.AddMap(new Repository.DataMapper.PlatformBilletMap());
                config.AddMap(new Repository.DataMapper.PlatformBilletBankDataMap());
                config.AddMap(new Repository.DataMapper.PlatformSubWorkflowMap());
                config.AddMap(new Repository.DataMapper.PlatformWorkflowMap());
                config.AddMap(new Repository.DataMapper.RealEstateMap());
                config.AddMap(new Repository.DataMapper.RealEstateBrokerMap());
                config.AddMap(new Repository.DataMapper.RegistryMap());
                config.AddMap(new Repository.DataMapper.SendInviteMap());
                config.AddMap(new Repository.DataMapper.SendInviteTextTypeMap());
                config.AddMap(new Repository.DataMapper.TransactionMap());
                config.AddMap(new Repository.DataMapper.TransactionDocumentMap());
                config.AddMap(new Repository.DataMapper.UserMap());
                config.AddMap(new Repository.DataMapper.UserBankDataMap());
                config.AddMap(new Repository.DataMapper.UserDocumentMap());
                config.AddMap(new Repository.DataMapper.UserProfileMap());
                config.AddMap(new Repository.DataMapper.UserSpouseMap());
                config.AddMap(new Repository.DataMapper.UserSpouseTypeMap());
            });
            services.AddSingleton<Repository.IAddressRepository, Repository.Impl.AddressRepository>();
            services.AddSingleton<Repository.IBankRepository, Repository.Impl.BankRepository>();
            services.AddSingleton<Repository.IBrokerRepository, Repository.Impl.BrokerRepository>();
            services.AddSingleton<Repository.ICityRepository, Repository.Impl.CityRepository>();
            services.AddSingleton<Repository.IDocumentRepository, Repository.Impl.DocumentRepository>();
            services.AddSingleton<Repository.IDocumentTypeRepository, Repository.Impl.DocumentTypeRepository>();
            services.AddSingleton<Repository.INotificationRepository, Repository.Impl.NotificationRepository>();
            services.AddSingleton<Repository.IPatrimonyRepository, Repository.Impl.PatrimonyRepository>();
            services.AddSingleton<Repository.IPatrimonyAcquirerTypeRepository, Repository.Impl.PatrimonyAcquirerTypeRepository>();
            services.AddSingleton<Repository.IPatrimonyDocumentRepository, Repository.Impl.PatrimonyDocumentRepository>();
            services.AddSingleton<Repository.IPatrimonyTransmitterTypeRepository, Repository.Impl.PatrimonyTransmitterTypeRepository>();
            services.AddSingleton<Repository.IPlatformBilletRepository, Repository.Impl.PlatformBilletRepository>();
            services.AddSingleton<Repository.IPlatformBilletBankDataRepository, Repository.Impl.PlatformBilletBankDataRepository>();
            services.AddSingleton<Repository.IPlatformSubWorkflowRepository, Repository.Impl.PlatformSubWorkflowRepository>();
            services.AddSingleton<Repository.IPlatformWorkflowRepository, Repository.Impl.PlatformWorkflowRepository>();
            services.AddSingleton<Repository.IRealEstateRepository, Repository.Impl.RealEstateRepository>();
            services.AddSingleton<Repository.IRealEstateBrokerRepository, Repository.Impl.RealEstateBrokerRepository>();
            services.AddSingleton<Repository.IRegistryRepository, Repository.Impl.RegistryRepository>();
            services.AddSingleton<Repository.ISendInviteRepository, Repository.Impl.SendInviteRepository>();
            services.AddSingleton<Repository.ISendInviteTextTypeRepository, Repository.Impl.SendInviteTextTypeRepository>();
            services.AddSingleton<Repository.ITransactionRepository, Repository.Impl.TransactionRepository>();
            services.AddSingleton<Repository.ITransactionDocumentRepository, Repository.Impl.TransactionDocumentRepository>();
            services.AddSingleton<Repository.IUserRepository, Repository.Impl.UserRepository>();
            services.AddSingleton<Repository.IUserBankDataRepository, Repository.Impl.UserBankDataRepository>();
            services.AddSingleton<Repository.IUserDocumentRepository, Repository.Impl.UserDocumentRepository>();
            services.AddSingleton<Repository.IUserProfileRepository, Repository.Impl.UserProfileRepository>();
            services.AddSingleton<Repository.IUserSpouseRepository, Repository.Impl.UserSpouseRepository>();
            services.AddSingleton<Repository.IUserSpouseTypeRepository, Repository.Impl.UserSpouseTypeRepository>();
        }
    }
}