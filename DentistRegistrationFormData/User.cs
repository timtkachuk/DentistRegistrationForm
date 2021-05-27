using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistRegistrationFormData
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Booking> ClientBookings { get; set; } = new HashSet<Booking>();
        public virtual ICollection<Booking> DoctorBookings { get; set; } = new HashSet<Booking>();
    }
}
