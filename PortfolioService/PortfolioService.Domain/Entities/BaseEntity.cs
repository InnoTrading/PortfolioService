using System.ComponentModel.DataAnnotations;

namespace PortfolioService.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public required string ID { get; set; }  
    }
}
