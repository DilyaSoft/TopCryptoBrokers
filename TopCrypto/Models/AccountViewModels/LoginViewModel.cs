using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TopCrypto.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
