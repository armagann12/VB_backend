using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
        
        [Required]
        [Range(0, 100.000)]
        public int Price { get; set; }

        [Required]
        [StringLength(100)]
        public string? Detail { get; set; }
        public bool Status { get; set; }
        public string? Month { get; set; }


        //BURAYA BAK 

        public int InstitutionModelId { get; set; }   //otomatik ekleyen kurum direk
        public InstitutionModel? InstitutionModel { get; set; } //otomatik ekleyen kurum direk
        public int UserModelId { get; set; } 
        public UserModel? UserModel { get; set; }
    }
}
