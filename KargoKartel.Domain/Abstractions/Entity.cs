using KargoKartel.Server.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace KargoKartel.Server.Domain.Abstractions
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.CreateVersion7();
        }

        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid CreatedBy { get; set; } = Guid.Empty;
        public string CreatedByName => GetCreatorUserName();

        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; } = null;
        public string? UpdatedByName => GetUpdaterUserName();

        public DateTimeOffset? DeletedAt { get; set; } = null;
        public Guid? DeletedBy { get; set; } = null;
        public string DeletedByName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        private string GetCreatorUserName()
        {
            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<UserManager<AppUser>>();

            AppUser appUser = userManager.Users.First(a => a.Id == CreatedBy);
            return $"{appUser.FirstName} {appUser.LastName} ({appUser.Email})".Trim();
        }
        private string? GetUpdaterUserName()
        {
            if (UpdatedBy == null)
                return null;
            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<UserManager<AppUser>>();
            AppUser appUser = userManager.Users.First(a => a.Id == UpdatedBy);
            return $"{appUser.FirstName} {appUser.LastName} ({appUser.Email})".Trim();
        }
    }
}
