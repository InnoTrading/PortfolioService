using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Domain.Managers;

namespace PortfolioService.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IPortfolioManager, PortfolioManager>();
            return services;
        }
    }
}