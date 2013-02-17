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
        public string Diagnosis;
        public Analysis()
        {
            Symptoms = new List<string>();
        }
    }
    public class Report
    {
        public string Patient { get; set; }
        public long Visits { get; set; }
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }
    }
}
