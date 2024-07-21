using System.ComponentModel.DataAnnotations;

namespace TopCrypto.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
