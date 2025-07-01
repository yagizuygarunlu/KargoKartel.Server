using KargoKartel.Server.Application.Services;
using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace KargoKartel.Server.Application.Auth
{
    public sealed record LoginCommand : IRequest<Result<LoginResponse>>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
    public sealed record LoginResponse
    {
        public string Token { get; init; } = string.Empty;
        public DateTime Expiration { get; init; }
        public LoginResponse(string token, DateTime expiration)
        {
            Token = token;
            Expiration = expiration;
        }
    }
    internal sealed class LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Result<LoginResponse>.Failure(404, "User not found");
            }
            var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signInResult.IsLockedOut)
            {
                TimeSpan? timeSpan = user.LockoutEnd - DateTimeOffset.UtcNow;
                if (timeSpan.HasValue)
                    return Result<LoginResponse>.Failure(403, $"User is locked out. Try again in {Math.Ceiling(timeSpan.Value.TotalMinutes)} minutes.");
                else
                    return Result<LoginResponse>.Failure(403, "User is locked out. Try again later.");

            }
            if (signInResult.IsNotAllowed)
            {
                return Result<LoginResponse>.Failure(403, "User is not allowed to sign in");
            }
            if (!signInResult.Succeeded)
            {
                return Result<LoginResponse>.Failure(401, "Invalid credentials");
            }
            var token = await jwtProvider.CreateTokenAsync(user, request.Password, cancellationToken);
            return new LoginResponse(token, DateTime.UtcNow.AddHours(1));
        }
    }
}
