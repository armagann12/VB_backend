using System.ComponentModel.DataAnnotations;

namespace InvoiceApi
{
    public class UserDto
    {
        [EmailAddress]
        public string Mail { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;


    }
}
