using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.Dto
{
    public class UserDto
    {
        [EmailAddress]
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


    }
}
