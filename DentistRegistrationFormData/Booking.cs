using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistRegistrationFormData
{
    public enum BookingStates
    {
        New, Approved, Cancelled
    }

    public class Booking
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        [Display(Name = "Doctor Name")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        public int DoctorId { get; set; }

        [Display(Name = "Procedure Name")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        public int ProcedureId { get; set; }

        [Display(Name = "Pick A Date")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        public DateTime dateTime { get; set; }
        public BookingStates BookingState { get; set; } = BookingStates.New;

        public virtual User Doctor { get; set; }
        public virtual User Client { get; set; }
        public virtual Procedure Procedure { get; set; }

    }
}
