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

namespace PatientRecordsWPF2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
        }

        /* PAGE INIT */
        private void wSearch_Loaded(object sender, RoutedEventArgs e)
        {
            var session = ((App)Application.Current).session;

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

    }
}
