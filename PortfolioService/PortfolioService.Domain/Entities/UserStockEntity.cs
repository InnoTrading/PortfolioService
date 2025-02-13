using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PortfolioService.Domain.Entities;

namespace PortfolioService.Domain.Entities
{
    public class UserStocks: BaseEntity
    {
        [Required]
        public required string UserID { get; set; }

        [Required]
        [ForeignKey(nameof(UserID))]
        public required virtual Users Users { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        public required string StockID { get; set; }

        [Required]
        [ForeignKey(nameof(StockID))]
        public required virtual Stocks Stock { get; set; }
    }
}
