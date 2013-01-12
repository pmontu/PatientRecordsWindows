using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Patient
    {
        public virtual Guid rowid { get; set; }
        public virtual string name { get; set; }
        public virtual int age { get; set; }
        public virtual string gender { get; set; }
    }
}
