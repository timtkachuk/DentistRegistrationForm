using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistRegistrationFormData
{
    public class Role : IdentityRole<int>
    {
        public string FriendlyName { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; } = new HashSet<Procedure>();
    }
}
