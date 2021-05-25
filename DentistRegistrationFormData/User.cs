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

        public virtual ICollection<Procedure> Procedures { get; set; } = new HashSet<Procedure>();
    }
}
