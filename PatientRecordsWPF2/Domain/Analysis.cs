using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWPF2.Domain
{
    public class Analysis
    {
        public List<string> Symptoms;
        public DateTime? From;
        public DateTime? To;
        public Analysis()
        {
            Symptoms = new List<string>();
        }
    }
}
