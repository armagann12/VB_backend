using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceApi.Models
{
    [Index(nameof(Mail), IsUnique = true)]
    public class InstitutionModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        public string? Detail { get; set; }

        [Required]
        [EmailAddress]
        public string? Mail { get; set; }

        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }
        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }


    }
}
