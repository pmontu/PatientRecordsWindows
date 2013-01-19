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
    /// Interaction logic for PatientDetails.xaml
    /// </summary>
    public partial class PatientDetails : Window
    {
        public PatientDetails()
        {
            InitializeComponent();
        }

        private void btnCreateNewPatient_Click(object sender, RoutedEventArgs e)
        {

            // VALIDATION
            bool isError = false;
            if (String.IsNullOrEmpty(this.txtName.Text)) 
            {
                txtName.BorderBrush = new BrushConverter().ConvertFromString("Red") as Brush;
                isError = true;
            }
            if (cbxSex.SelectedIndex == -1)
            {
                borderCbxSex.SetValue(Border.BorderBrushProperty, Brushes.Red);
                isError = true;
            }
            if (isError)
            {
                return;
            }

            // ENCAPSULATION
            var patient = new Domain.Patient();
            patient.Name = txtName.Text;
            patient.Phone = txtPhone.Text;
            patient.Pin = txtPin.Text;
            if (cbxSex.SelectedIndex != -1)
            {
                patient.Sex = (Domain.Sex)Enum.Parse(typeof(Domain.Sex), cbxSex.SelectedItem.ToString());
            }
            patient.State = txtState.Text;
            patient.Address = txtAddress.Text;
            patient.City = txtCity.Text;
            if (dtDate_of_Birth.SelectedDate.HasValue)
            {
                patient.Date_of_Birth = dtDate_of_Birth.SelectedDate.Value;
            }
            patient.Email = txtEmail.Text;
            patient.Father_or_Spouce = txtFather_or_spouce.Text;

            // DB
            var sessionFactory = ((App)Application.Current).sessionFactory; 

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(patient);
                    transaction.Commit();
                }
            }

            this.Close();
        }

        private void wDetails_Activated(object sender, EventArgs e)
        {
            cbxSex.ItemsSource = Enum.GetValues(typeof(Domain.Sex));
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtName.BorderBrush = new BrushConverter().ConvertFromString("#FFABADB3") as Brush;
        }

        private void cbxSex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            borderCbxSex.BorderBrush = new BrushConverter().ConvertFromString("#FFABADB3") as Brush;
        }

    }
}
