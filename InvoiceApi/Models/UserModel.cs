using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceApi.Models
{
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

        
        [StringLength(15)]
        
        public string? Password { get; set; }   //Password kaydolmucak db ye

        [Required]
        public int TC { get; set; }

        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }



        //public int invoiceId { get; set; } //array olucak burası
        //public InvoiceModel? InvoiceModel { get; set; }  //array olucak burası


    }
}
