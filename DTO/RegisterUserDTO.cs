﻿using System.ComponentModel.DataAnnotations;

namespace UserAPI_Dotnet8.DTO
{
    public class RegisterUserDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

    }
}
