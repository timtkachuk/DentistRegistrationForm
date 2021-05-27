using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistRegistrationForm.Models
{
    public class NewPasswordViewModel
    {
        [Display(Name = "New Password")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Repeat Password")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [Compare("Password", ErrorMessage = "{0} and {1} fields must be same!")]
        [DataType(DataType.Password)]
        public string PasswordVerify { get; set; }

        public string Token { get; set; }

        public string Id { get; set; }

    }
}
