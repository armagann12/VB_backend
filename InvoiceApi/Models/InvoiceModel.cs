using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.Models
{
    public class InvoiceModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string? name { get; set; }
        
        [Required]
        [Range(0, 100.000)]
        public int price { get; set; }

        [Required]
        [StringLength(100)]
        public string? detail { get; set; }
        public bool status { get; set; }
        public string? month { get; set; }


        //BURAYA BAK 

        public string? institutionId { get; set; }   //otomatik ekleyen kurum direk
        public InstitutionModel? InstitutionModel { get; set; } //otomatik ekleyen kurum direk
        public int userId { get; set; } 
        public UserModel? UserModel { get; set; }
    }
}
