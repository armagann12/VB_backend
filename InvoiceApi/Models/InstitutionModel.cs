using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.Models
{
    public class InstitutionModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? name { get; set; }

        [Required]
        [StringLength(100)]
        public string? detail { get; set; }

        [Required]
        [EmailAddress]
        public string? mail { get; set; }

        [Required]
        [StringLength(100)]
        public string? password { get; set; }    //Password kaydolmucak db ye


        //public int invoiceId { get; set; }
        //public InvoiceModel? InvoiceModel { get; set; }  

    }
}
