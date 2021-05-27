using System.ComponentModel.DataAnnotations;

namespace DentistRegistrationForm.Models
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
#if !DEBUG
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail adress!")]
        [DataType(DataType.EmailAddress)]
#endif
        public string UserName { get; set; }
    }
}
