using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.Models
{
    [Index(nameof(InvoiceNumber), IsUnique = true)]
    public class InvoiceModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string? Name { get; set; }

        public int InvoiceNumber { get; set; }
        
        [Required]
        [Range(0, 9999.000)]
        public int Price { get; set; }

        [Required]
        [StringLength(100)]
        public string? Detail { get; set; }
        public bool Status { get; set; }
        public string? Month { get; set; }

        public int InstitutionModelId { get; set; }
        public InstitutionModel? InstitutionModel { get; set; } 
        public int UserModelId { get; set; } 
        public UserModel? UserModel { get; set; }
    }
}
