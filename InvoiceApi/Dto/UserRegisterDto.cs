using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace InvoiceApi.Dto
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
        public string Mail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Range(10000000000, 99999999999)]
        [Required]
        public long TC { get; set; }
    }
}
