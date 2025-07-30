using Microsoft.Extensions.DependencyInjection;
using OpenCredentialPublisher.Proof;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Interfaces;

namespace OpenCredentialPublisher.DependencyInjection
{
    public static class RegisterServices
    {
        public static IServiceCollection AppServiceRegistration(IServiceCollection services)
        {
            CommonServices(services);

            services.AddTransient<ConnectService>();
            services.AddTransient<SetupService>();

            services.AddTransient<LogHttpClientService>();
            services.AddTransient<ProfileImageService>();

            return services;
        }


        private static IServiceCollection CommonServices(IServiceCollection services)
        {
            services.AddTransient<AzureBlobStoreService>();

            services.AddTransient<EmailService>();
            services.AddTransient<LogHttpClientService>();
            services.AddTransient<SchemaService>();
            services.AddTransient<ISchemaService, SchemaService>();
            services.AddTransient<ETLService>();
            services.AddTransient<CredentialPackageService>();
            services.AddTransient<ShareService>();
            services.AddTransient<CredentialCollectionService>();
            services.AddTransient<CredentialService>();
            services.AddTransient<NotificationService>();
            services.AddTransient<EvidenceService>();
            services.AddTransient<ProofService>();
            services.AddTransient<IProofService, ProofService>();
            services.AddTransient<RevocationService>();
            services.AddTransient<IRevocationService, RevocationService>();

            return services;
        }
    }
}
