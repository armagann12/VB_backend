using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.Json.Serialization;

namespace InvoiceApi.Models
{
    [Index(nameof(Mail), IsUnique = true)]
    [Index(nameof(TC), IsUnique = true)]
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Mail { get; set; }

        [Range(10000000000, 99999999999)]
        [Required]
        public long TC { get; set; }

        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }
        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }

    }
}
