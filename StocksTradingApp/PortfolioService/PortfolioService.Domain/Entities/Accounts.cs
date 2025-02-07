using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PortfolioService.Domain.Entities
{
    public class Accounts: Base
    {
        [Required]
        public required string UserID { get; set; }

        [Required]
        [ForeignKey(nameof(UserID))]
        public required virtual Users User { get; set; }

        [Required]
        public required decimal Balance { get; set; }

        [Required]
        public required decimal ReservedBalance { get; set; }

    }
}
