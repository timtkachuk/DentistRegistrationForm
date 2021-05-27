using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistRegistrationFormData
{
    public class Booking
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int DoctorId { get; set; }
        public int ProcedureId { get; set; }
        public DateTime DateTime { get; set; }

        public virtual User Doctor { get; set; }
        public virtual User Client { get; set; }
        public virtual Procedure Procedure { get; set; }

    }
}
