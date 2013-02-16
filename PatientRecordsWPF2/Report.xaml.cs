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
        private Domain.Analysis Analysis;

        public Report()
        {
            InitializeComponent();
        }
        public Report(Domain.Analysis analysis)
            : this()
        {
            Analysis = analysis;
        }

        private void wReport_Loaded(object sender, RoutedEventArgs e)
        {
            var session = ((App)Application.Current).session;

            string inner = "";
            foreach (string s in Analysis.Symptoms)
            {
                if (inner == "")
                {
                    inner += "select visit_id from symptom where name = '" + s + "'";
                }
                else
                {
                    inner += " intersect select visit_id from symptom where name = '" + s + "'";
                }
            }

            string f = DateTime.Parse(Analysis.From.ToString()).ToString("yyyy-MM-dd");
            string t = DateTime.Parse(Analysis.To.ToString()).ToString("yyyy-MM-dd");

            IQuery q = session.CreateSQLQuery("select pname as patient,count(distinct(dt)) as visits,group_concat(distinct(sym)) as symptoms "+
                                                "from "+
                                                "("+
                                                "select name as sym,visit_id from symptom where visit_id in "+
                                                "(" + inner + ") " +
                                                ") as s, "+
                                                "( "+
                                                "select "+
                                                "       (select name from patient where id = visit.patient_id) as pname, "+
                                                "       date_of_examination as dt, "+
                                                "       id as visit_id "+
                                                "from visit "+
                                                "where id in (" + inner + ") " +
                                                ") as v "+
                                                "where s.[visit_id] = v.[visit_id] and date(dt) between date('"+f+"') and date('"+t+"') " +
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
