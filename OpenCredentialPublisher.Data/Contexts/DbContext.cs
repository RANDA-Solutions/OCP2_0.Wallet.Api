using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenCredentialPublisher.Shared.Interfaces;

namespace OpenCredentialPublisher.Data.Contexts
{
    public abstract class DbContext : IdentityDbContext<ApplicationUser>
    {
        protected DbContext(
            DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected DbContext(DbContextOptions options)
            : base(options)
        {
        }


        /// <summary>
        /// Log for HttpClient calls
        /// </summary>
        public DbSet<HttpClientLog> HttpClientLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatusModel>(eb => {
                eb.Property(s => s.Id)
                    .HasConversion<int>();

                eb.HasData(Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().Select(s => new StatusModel { Id = s, Name = s.ToString() }));
            });

            base.OnModelCreating(modelBuilder);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync(true);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            ChangeTracker.DetectChanges();

            Timestamp(now);

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }
        private void Timestamp(DateTime now)
        {
            var addedEntities = ChangeTracker.Entries<IBaseEntity>().Where(e => e.State == EntityState.Added).ToList();

            addedEntities.ForEach(e =>
            {
                e.Entity.CreatedAt = now;
                e.Entity.ModifiedAt = now;
            });

            var editedEntities = ChangeTracker.Entries<IBaseEntity>().Where(e => e.State == EntityState.Modified).ToList();

            editedEntities.ForEach(e =>
            {
                e.Entity.ModifiedAt = now;
            });
        }
    }
}
