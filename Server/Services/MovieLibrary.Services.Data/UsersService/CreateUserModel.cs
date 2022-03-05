namespace MovieLibrary.Services.Data.UsersService
{
    using MovieLibrary.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class CreateUserModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Name must contains only letters")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [RegularExpression("[A-Za-z]+", ErrorMessage = "Name must contains only letters")]
        public string LastName { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
