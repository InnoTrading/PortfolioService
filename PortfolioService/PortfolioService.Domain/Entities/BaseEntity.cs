using System.ComponentModel.DataAnnotations;

namespace PortfolioService.Domain.Entities
{
    public class BaseEntity
    {
        public Guid ID { get; set; } = Guid.NewGuid();
    }
}

