using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Application.Services;
using PortfolioService.Domain.Interfaces;

namespace PortfolioService.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountManager, AccountManager>();
            return services;
        }
    }
}
