using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace InvoiceApi
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(15)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;

        [Range(10000000000, 99999999999)]
        [Required]
        public long TC { get; set; }
    }
}
