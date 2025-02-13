using System.ComponentModel.DataAnnotations;

namespace PortfolioService.Domain.Entities
{
    public class Users : Base
    {
        [Required]
        [StringLength(50)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public required string UserEmail { get; set; }
    }
}
