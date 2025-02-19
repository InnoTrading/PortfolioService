using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Application.Interfaces;
using PortfolioService.Application.Services;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}
