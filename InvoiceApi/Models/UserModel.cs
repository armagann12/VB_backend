using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string? firstName { get; set; }

        [Required]
        [StringLength(15)]
        public string? lastName { get; set; }

        [Required]
        [EmailAddress]
        public string? mail { get; set; }

        [Required]
        [StringLength(15)]
        [JsonIgnore]
        public string? password { get; set; }   //Password kaydolmucak db ye

        [Required]
        public int TC { get; set; }



        //public int invoiceId { get; set; } //array olucak burası
        //public InvoiceModel? InvoiceModel { get; set; }  //array olucak burası


    }
}
