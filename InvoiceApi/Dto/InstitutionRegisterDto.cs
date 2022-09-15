using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.Dto
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
        public string Mail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
