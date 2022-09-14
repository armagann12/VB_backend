using System.ComponentModel.DataAnnotations;

namespace InvoiceApi
{
    public class InstitutionRegisterDto
    {
        [Required]
        [StringLength(20)]
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        public string? Detail { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
