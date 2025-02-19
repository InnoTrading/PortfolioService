using Microsoft.AspNetCore.Authentication.JwtBearer;
using PortfolioService.Application.Extensions;
using PortfolioService.Domain.Extensions;
using PortfolioService.Infrastructure.Extensions;
using PortfolioService.Infrastructure.Messaging;
using PortfolioService.WebAPI.Extensions;
using PortfolioService.WebAPI.Middlewares;
using ApplicationExtensions = PortfolioService.Application.Extentions.ApplicationExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
PresentationExtensions.AddPresentationServices(builder.Services);
ApplicationExtensions.AddApplicationServices(builder.Services);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddDomainServices();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-y03fg5cbt3pqn8o8.us.auth0.com/";
    options.Audience = "https://inno-trading-auth";
});


var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.ocelo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
  
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();