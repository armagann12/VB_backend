using System.ComponentModel.DataAnnotations;

namespace InvoiceApi.Dto
{
    public class InstitutionDto
    {
        [EmailAddress]
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


    }
}
