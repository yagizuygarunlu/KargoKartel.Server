using KargoKartel.Server.Domain.Common;
using KargoKartel.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace KargoKartel.Server.Application.Auth
{
    public sealed record RegisterCommand:IRequest<Result<string>>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
    }

    internal sealed class RegisterCommandHandler(UserManager<AppUser> userManager)
        : IRequestHandler<RegisterCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Result<string>.Failure("A user with this email already exists.");
            }

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<string>.Failure(errors);
            }

            return Result<string>.Succeed(user.Id.ToString());
        }
    }
}
