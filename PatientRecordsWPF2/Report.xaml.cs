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
            if (!String.IsNullOrEmpty(Analysis.Diagnosis))
            {
                if (inner == "")
                {
                    inner += "select id from visit where diagnosis = '" + Analysis.Diagnosis + "'";
                }
                else
                {
                    inner += " intersect select id from visit where diagnosis = '" + Analysis.Diagnosis + "'";
                }
            }

            /*removing where clause if no symptoms or diagnosis*/
            string w1="", w2="";
            if (inner != "")
            {
                w1 = "where id in (" + inner + ")";
                w2 = "where visit_id in (" + inner + ")";
            }

            string f = DateTime.Parse(Analysis.From.ToString()).ToString("yyyy-MM-dd");
            string t = DateTime.Parse(Analysis.To.ToString()).ToString("yyyy-MM-dd");

            /*
             * Left join two tables - to get 
             *      number of visits: distinct datesofexamination
             *      group of accompaniing symptoms in all those visits
             *      diagnosis in all those visits
             * one - visits which have symptoms and diagnosis
             * two - symptoms in a visit with symptoms and diagnosis
             * 
             * Grouped by patient name, 
             * orderd by number of visits and 
             * filted by date*/

            IQuery q = session.CreateSQLQuery("select pname as patient,"+
                                                "   count(distinct(dt)) as visits,"+
                                                "   case when (group_concat(distinct(sym))) is null then '' else group_concat(distinct(sym)) end as symptoms,"+
                                                "   case when (group_concat(distinct(diagnosis))) is null then '' else group_concat(distinct(diagnosis)) end as diagnosis " +
                                                "from "+
                                                "( " +
                                                "   select " +
                                                "           (select name from patient where id = visit.patient_id) as pname, " +
                                                "           date_of_examination as dt, " +
                                                "           id as visit_id, " +
                                                "           diagnosis " +
                                                "   from visit " +
                                                w1 +
                                                ") as v " +
                                                "left join "+
                                                "("+
                                                "   select name as sym,visit_id from symptom "+
                                                w2 +
                                                ") as s "+                                                
                                                "on s.[visit_id] = v.[visit_id] "+
                                                "where date(dt) between date('"+f+"') and date('"+t+"') " +
                                                "group by pname "+
                                                "order by visits desc,patient ");

            IList<object> o = q.List<object>();

            var r = o
              .Select(x => new Domain.Report() { 
                  Patient = (string)((object[])x)[0], 
                  Visits = (long)((object[])x)[1], 
                  Symptoms = (string)((object[])x)[2], 
                  Diagnosis = (string)((object[])x)[3] 
              })
              .ToList();

            dgReport.ItemsSource = r;

        }
    }
}
