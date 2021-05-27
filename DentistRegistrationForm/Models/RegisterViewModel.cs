using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistRegistrationForm.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail adress")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Password Verify")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        [Compare("Password", ErrorMessage = "{0} and {1} fields must be the same")]
        [DataType(DataType.Password)]
        public string PasswordVerify { get; set; }

        [Display(Name = "Name/Last Name")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
