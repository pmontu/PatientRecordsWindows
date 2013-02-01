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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using NHibernate.Criterion;
using System.IO;

namespace PatientRecordsWPF2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ISession session { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            session = ((App)Application.Current).session;
        }

        /* VIEW PATIENT VISITS */

        private void btnViewPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            ViewPatientVisits();
        }

        private void ViewPatientVisits()
        {
            if (lbxPatients.SelectedIndex != -1)
            {
                var pv = new PatientVisits((Domain.Patient)lbxPatients.SelectedItem);
                pv.Owner = this;
                pv.ShowDialog();
            }
        }

        /* ADD NEW PATIENT */
        private void btnAddNewPatient_Click(object sender, RoutedEventArgs e)
        {
            var pd = new PatientDetails();
            pd.Owner = this;
            pd.ShowDialog();
            load();
        }

        /* PAGE INIT */
        private void wSearch_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }

        private void cleanup()
        {
            /* Paths of media saved on db */
            List<String> existingmediapaths = session.CreateCriteria<Domain.Medium>()
                .SetProjection(Projections.Property("Path"))
                .List<string>().Select(p => p.Replace(@"Media\","")).ToList();
            /* files in directory */
            List<String> existingfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Media")
                .Select(path => System.IO.Path.GetFileName(path))
                .ToList();
            /* removing thumnails from list */
            existingfiles.RemoveAll(path => path.Contains(@"-thumbnail.jpg"));
            /* optimisation */
            existingfiles.Sort();
            existingmediapaths.Sort();
            /* search for files not in db */
            foreach (string e in existingfiles)
            {
                bool isFilePresentInDB = false;
                foreach (string p in existingmediapaths)
                {
                    if (string.Compare(e, p) < 0)
                        break;
                    else if (e == p)
                        isFilePresentInDB = true;                    
                }
                if (!isFilePresentInDB)
                {
                    if(e.Contains(".wmv"))
                    {
                        File.Delete(Directory.GetCurrentDirectory() + @"\Media\" + e + @"-thumbnail.jpg");
                    }
                    File.Delete(Directory.GetCurrentDirectory() + @"\Media\" + e);
                }
            }
        }

        private void load()
        {
            var patients = session.CreateCriteria<Domain.Patient>().List<Domain.Patient>();

            lbxPatients.ItemsSource = patients;

            /* keyboard focus on search filter textbox */
            Keyboard.Focus(txtSearchFilter);   
        }
        
        /* SEARCH FILTER */
        private void txtSearchFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbxPatients.Items.Filter = a =>
            {
                if (((Domain.Patient)a).Name.ToString().ToLower().Contains(txtSearchFilter.Text.ToLower()))
                {
                    return true;
                }
                return false;
            };
        }

        private void lbxPatients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewPatientVisits();
        }

        private void wSearch_Closed(object sender, EventArgs e)
        {
            cleanup();
        }


    }
}
