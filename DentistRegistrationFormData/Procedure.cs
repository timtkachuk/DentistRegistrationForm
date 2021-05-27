using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistRegistrationFormData
{
    public class Procedure
    {
        public int Id { get; set; }

        [Display(Name = "Procedure Name")]
        [Required(ErrorMessage = "{0} field cannot be empty!")]
        public string Name { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
