using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Custom.EFModels;
using OpenCredentialPublisher.Data.Custom.EntityTypeConfigurations;
using DbContext = OpenCredentialPublisher.Data.Contexts.DbContext;

namespace OpenCredentialPublisher.Data.Custom.Contexts
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations for v2.
            modelBuilder
                .ApplyConfiguration(new ApplicationUserConfiguration())
                .ApplyConfiguration(new NotificationConfiguration())
                .ApplyConfiguration(new CredentialPackageConfiguration())
                .ApplyConfiguration(new ProfileConfiguration())
                .ApplyConfiguration(new VerifiableCredentialConfiguration())
                .ApplyConfiguration(new NotificationConfiguration())
                .ApplyConfiguration(new AchievementConfiguration())
                .ApplyConfiguration(new AchievementAlignmentConfiguration())
                .ApplyConfiguration(new AssociationConfiguration())
                .ApplyConfiguration(new SearchCredentialPackageConfiguration())
                .ApplyConfiguration(new SearchCredentialPackageAchievementTypeConfiguration())
                .ApplyConfiguration(new SearchCredentialPackageIssuerConfiguration())
                .ApplyConfiguration(new SearchCredentialConfiguration())
                .ApplyConfiguration(new SearchCredentialCollectionConfiguration())
                .ApplyConfiguration(new CredentialCollectionConfiguration())
                .ApplyConfiguration(new CredentialCollectionVerifiableCredentialConfiguration())
                .ApplyConfiguration(new AchievementIdentityConfiguration())
                .ApplyConfiguration(new SetupConfiguration())
                .ApplyConfiguration(new ShareConfiguration())
                .ApplyConfiguration(new ShareVerifiableCredentialConfiguration())
                .ApplyConfiguration(new ShareCredentialCollectionConfiguration())
                .ApplyConfiguration(new EvidenceConfiguration())
                .ApplyConfiguration(new AuthorizationConfiguration())
                .ApplyConfiguration(new MessageConfiguration())
                .ApplyConfiguration(new SourceConfiguration())
                .ApplyConfiguration(new ResultConfiguration());

        }


        public DbSet<Message> Messages { get; set; }

        //public DbSet<VerifiableCredential> VerifiableCredentials2 { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        
        
        /// <summary>
        /// Known resource servers.
        /// </summary>
        public DbSet<Source> Sources { get; set; }

        public DbSet<CredentialPackage> CredentialPackages2 { get; set; }
        public DbSet<VerifiableCredential> VerifiableCredentials2 { get; set; }
        public DbSet<Profile> Profiles2 { get; set; }
        public DbSet<Achievement> Achievements2 { get; set; }
        public DbSet<AchievementIdentity> AchievementIdentities2 { get; set; }
        public DbSet<CredentialCollection> CredentialCollection2 { get; set; }
        public DbSet<Association> Associations2 { get; set; } // Keeping the 2 for now for consistency

        public DbSet<SearchCredentialPackage> SearchCredentialPackages { get; set; }
        public DbSet<SearchCredential> SearchCredentials { get; set; }
        public DbSet<SearchCredentialCollection> SearchCredentialCollections { get; set; }

        
        public DbSet<Setup> Setups { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<ShareVerifiableCredential> ShareVerifiableCredentials2 { get; set; }
        public DbSet<ShareCredentialCollection> ShareCredentialCollections2 { get; set; }
        public DbSet<Evidence> Evidences2 { get; set; }
        public DbSet<Result> Results2 { get; set; }
    }
}
