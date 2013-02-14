using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWPF2.Model
{
    class Analysis
    {
        private Domain.Analysis Entity;

        public void init()
        {
            Entity = new Domain.Analysis();
        }

        public void addSymptom(string s)
        {
            Entity.Symptoms.Add(s);
        }

        public void removeSymptom(string s)
        {
            Entity.Symptoms.Remove(s);
        }

        public void setDates(DateTime f, DateTime t)
        {
            Entity.From = f;
            Entity.To = t;
        }

        public List<String> getSymptoms()
        {
            return Entity.Symptoms;
        }
    }
}
