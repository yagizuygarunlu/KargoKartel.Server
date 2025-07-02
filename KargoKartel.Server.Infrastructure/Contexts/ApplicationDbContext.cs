using KargoKartel.Server.Domain.Abstractions;
using KargoKartel.Server.Domain.Cargos;
using KargoKartel.Server.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KargoKartel.Server.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cargo> Cargos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Ignore<IdentityUserClaim<Guid>>();
            modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserToken<Guid>>();
            modelBuilder.Ignore<IdentityUserLogin<Guid>>();
            modelBuilder.Ignore<IdentityUserRole<Guid>>();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Entity>();

            HttpContextAccessor httpContextAccessor = new();
            string? userIdString =
                httpContextAccessor
                .HttpContext!
                .User
                .Claims
                .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?
                .Value;

            _ = Guid.TryParse(userIdString, out Guid userId);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt)
                        .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.CreatedBy)
                        .CurrentValue = userId;
                }

                if (entry.State == EntityState.Modified)
                {
                    if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                    {
                        entry.Property(p => p.DeletedAt)
                        .CurrentValue = DateTimeOffset.Now;
                        entry.Property(p => p.DeletedBy)
                        .CurrentValue = userId;
                    }
                    else
                    {
                        entry.Property(p => p.UpdatedAt)
                            .CurrentValue = DateTimeOffset.Now;
                        entry.Property(p => p.UpdatedBy)
                        .CurrentValue = userId;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
