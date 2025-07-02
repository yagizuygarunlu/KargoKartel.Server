using Azure.Core;
using KargoKartel.Server.Application.Auth;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace KargoKartel.Server.WebAPI.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/v1").WithTags("Auth");
            group.MapPost("/login", async ([FromServices] ISender sender,[FromBody] LoginCommand request, CancellationToken cancellatioNToken) =>
            {
                var response = await sender.Send(request, cancellatioNToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .WithName("Login");
            group.MapPost("/register", async ([FromServices] ISender sender, [FromBody] RegisterCommand request, CancellationToken cancellatioNToken) =>
            {
                var response = await sender.Send(request, cancellatioNToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .WithName("Register");
            //group.MapPost("/logout", async (HttpContext context) =>
            //{
            //    // Implement logout logic here
            //    return Results.Ok("Logout successful");
            //})
            //.WithName("Logout");
        }
    }
}
