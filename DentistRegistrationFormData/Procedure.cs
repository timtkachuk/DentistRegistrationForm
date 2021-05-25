﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistRegistrationFormData
{
    public class Procedure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual User DoctorId { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
