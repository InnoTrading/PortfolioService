using System.ComponentModel.DataAnnotations;

namespace PortfolioService.Domain.Entities
{
    public class Base
    {
        [Key]
        public required string ID { get; set; }  
    }
}
