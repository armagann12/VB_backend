using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceApi.Models
{
    public class InstitutionModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        public string? Detail { get; set; }

        [Required]
        [EmailAddress]
        public string? Mail { get; set; }

        
        [StringLength(100)]
        public string? Password { get; set; }    //Password kaydolmucak db ye


        //public int invoiceId { get; set; }
        //public InvoiceModel? InvoiceModel { get; set; }  

    }
}
