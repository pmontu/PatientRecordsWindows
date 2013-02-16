using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PatientRecordsWPF2
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        public Report()
        {
            InitializeComponent();
        }

        private void wReport_Loaded(object sender, RoutedEventArgs e)
        {
            var session = ((App)Application.Current).session;

            IQuery q = session.CreateSQLQuery("select pname as patient,count(distinct(dt)) as visits,group_concat(distinct(sym)) as symptoms "+
                                                "from "+
                                                "("+
                                                "select name as sym,visit_id from symptom where visit_id in "+
                                                "(select visit_id from symptom where name = 'ajay_v2_s1') "+
                                                ") as s, "+
                                                "( "+
                                                "select "+
                                                "       (select name from patient where id = visit.patient_id) as pname, "+
                                                "       date_of_examination as dt, "+
                                                "       id as visit_id "+
                                                "from visit "+
                                                "where id in (select visit_id from symptom where name = 'ajay_v2_s1') "+
                                                ") as v "+
                                                "where s.[visit_id] = v.[visit_id] "+
                                                "group by pname "+
                                                "order by visits desc,patient ");

            IList<object> o = q.List<object>();

            var r = o
              .Select(x => new Domain.Report() { Patient = (string)((object[])x)[0], Visits = (long)((object[])x)[1], Symptoms = (string)((object[])x)[2] })
              .ToList();

            dgReport.ItemsSource = r;

        }
    }
}
