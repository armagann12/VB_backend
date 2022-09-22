using Microsoft.EntityFrameworkCore;

namespace InvoiceApi.Models
{
    [Index(nameof(Number), IsUnique = true)]
    public class CreditCardModel
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public string? UserName { get; set; }
        public int CVC { get; set; }
        public string? ValidDate { get; set; }
        public string? BankName { get; set; }
        public long Balance { get; set; }
        public int UserModelId { get; set; }
        public UserModel? UserModel { get; set; }
    }
}
