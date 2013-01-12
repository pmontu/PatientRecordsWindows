using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWindows.Domain
{
    public class Patient
    {
        public virtual Guid Id { get; set; }
        public virtual string name { get; set; }
        public virtual int age { get; set; }
        public virtual string gender { get; set; }
    }
}
