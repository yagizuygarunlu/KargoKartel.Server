using KargoKartel.Server.Domain.Users;

namespace KargoKartel.Server.Application.Services
{
    public interface IJwtProvider
    {
        public Task<string> CreateTokenAsync(AppUser user, string password, CancellationToken cancellationToken = default);
    }
}
