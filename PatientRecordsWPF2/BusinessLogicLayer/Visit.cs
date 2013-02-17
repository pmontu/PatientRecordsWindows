using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PatientRecordsWPF2.BusinessLogicLayer
{
    public class Visit
    {
        public static IList<String> getDiagnosisDb()
        {
            var session = ((App)Application.Current).session;
            return session.CreateCriteria(typeof(Domain.Visit))
                .SetProjection(Projections.Distinct(Projections.ProjectionList().Add(Projections.Alias(Projections.Property("Diagnosis"), "Diagnosis"))))
                .List<String>();
        }
    }
}
