using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PatientRecordsWPF2.BusinessLogicLayer
{
    public class Symptom
    {
        public static IList<String> getSymptomsDb()
        {
            var session = ((App)Application.Current).session;
            return session.CreateCriteria(typeof(Domain.Symptom))
                .SetProjection(Projections.Distinct(Projections.ProjectionList().Add(Projections.Alias(Projections.Property("Name"), "Name"))))
                .List<String>();
        }
    }
}
