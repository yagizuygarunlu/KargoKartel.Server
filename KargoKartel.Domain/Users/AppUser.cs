﻿using Microsoft.AspNetCore.Identity;

namespace KargoKartel.Server.Domain.Users
{
    public sealed class AppUser: IdentityUser<Guid>
    {
        public AppUser()
        {
            Id = Guid.CreateVersion7();
        }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public DateTimeOffset CreateAt { get; set; }
        public Guid CreateUserId { get; set; } = default!;
        public DateTimeOffset? UpdateAt { get; set; }
        public Guid? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeleteAt { get; set; }
        public Guid? DeleteUserId { get; set; }
    }
}
