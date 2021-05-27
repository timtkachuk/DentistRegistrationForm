using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentistRegistrationForm.Models
{
    public class NewBookingViewModel
    {
        [Display(Name ="Doctor Name")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        public int DoctorId { get; set; }

        [Display(Name = "Procedure Name")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        public int ProcedureId { get; set; }

        [Display(Name = "Pick A Date")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        public DateTime dateTime { get; set; }
    }
}
