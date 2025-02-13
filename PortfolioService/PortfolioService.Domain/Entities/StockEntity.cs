using System.ComponentModel.DataAnnotations;

namespace PortfolioService.Domain.Entities
{
    public class Stocks : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(10)]
        public required string Ticker { get; set; }

        [Required]
        public required decimal Price { get; set; }

        [Required]
        public required string Description { get; set; }
    }
}
