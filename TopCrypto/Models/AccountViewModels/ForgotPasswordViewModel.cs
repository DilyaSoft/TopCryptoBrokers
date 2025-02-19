﻿using System.ComponentModel.DataAnnotations;

namespace TopCrypto.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
