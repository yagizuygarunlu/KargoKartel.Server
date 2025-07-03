using KargoKartel.Server.Application;
using KargoKartel.Server.Infrastructure;
using KargoKartel.Server.WebAPI;
using KargoKartel.Server.WebAPI.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});


builder.Services.AddCors();
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromMinutes(1);
    });
});

var app = builder.Build();
app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.UseResponseCompression();

app.UseExceptionHandler();
app.MapDefaultEndpoints();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Uncomment if you have controllers
// app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();

app.MapAuthEndpoints();
app.MapCargoEndpoints();

app.Run();
