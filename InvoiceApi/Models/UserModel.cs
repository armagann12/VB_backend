using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.Json.Serialization;

namespace InvoiceApi.Models
{
    [Index(nameof(Mail), IsUnique = true)]
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

        [Required]
        public long TC { get; set; }

        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }
        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }

    }
}
