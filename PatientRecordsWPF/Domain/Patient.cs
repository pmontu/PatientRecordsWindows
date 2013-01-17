using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWPF.Domain
{
    public class Patient
    {
        public virtual Guid id { get; set; }
        public virtual string name { get; set; }
        public virtual string fatherorspouce { get; set; }
        public virtual string sex { get; set; }
        public virtual DateTime dateofbirth { get; set; }
        public virtual string address_number { get; set; }
        public virtual string address_street { get; set; }
        public virtual string address_area { get; set; }
        public virtual string city { get; set; }
        public virtual string state { get; set; }
        public virtual string pin { get; set; }
        public virtual string phone { get; set; }
        public virtual string email { get; set; }
        public virtual string diabetes { get; set; }
        public virtual string bloodpressure { get; set; }
        public virtual string heartailments { get; set; }
        public virtual string tb { get; set; }
        public virtual string aids { get; set; }
        public virtual string allergy { get; set; }
        public virtual string other { get; set; }
    }
}
