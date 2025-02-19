using System.ComponentModel.DataAnnotations;

namespace PortfolioService.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}