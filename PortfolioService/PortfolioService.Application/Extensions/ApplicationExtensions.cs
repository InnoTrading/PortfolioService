using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Application.Interfaces;
using PortfolioService.Application.Services;

namespace PortfolioService.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPortfolioService, Services.PortfolioService>();
            return services;
        }
    }
}