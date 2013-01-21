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
        private Domain.Patient Patient;
        public PatientDetails()
        {
            InitializeComponent();
        }
        public PatientDetails(Domain.Patient Patient)
            : this()
        {
            this.Patient = Patient;
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
            Patient.Name = txtName.Text;
            Patient.Phone = txtPhone.Text;
            Patient.Pin = txtPin.Text;
            if (cbxSex.SelectedIndex != -1)
            {
                Patient.Sex = (Domain.Sex)Enum.Parse(typeof(Domain.Sex), cbxSex.SelectedItem.ToString());
            }
            Patient.State = txtState.Text;
            Patient.Address = txtAddress.Text;
            Patient.City = txtCity.Text;
            if (dtDate_of_Birth.SelectedDate.HasValue)
            {
                Patient.Date_of_Birth = dtDate_of_Birth.SelectedDate.Value;
            }
            Patient.Email = txtEmail.Text;
            Patient.Father_or_Spouce = txtFather_or_spouce.Text;

            // DB
            var session = ((App)Application.Current).session;

            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(Patient);
                transaction.Commit();
            }

            this.Close();
        }

        private void wDetails_Activated(object sender, EventArgs e)
        {
            cbxSex.ItemsSource = Enum.GetValues(typeof(Domain.Sex));
            if (Patient != null)
            {
                lblTitle.Content = "Patient Details " + Patient.Id;
                btnCreateNewPatient.Content = "Update";

                txtName.Text = Patient.Name;
                txtPhone.Text = Patient.Phone;
                txtPin.Text = Patient.Pin;
                cbxSex.SelectedIndex = cbxSex.Items.IndexOf(Patient.Sex);
                txtState.Text = Patient.State;
                txtAddress.Text = Patient.Address;
                txtCity.Text = Patient.City;
                dtDate_of_Birth.Text = Patient.Date_of_Birth.ToString();
                txtEmail.Text = Patient.Email;
                txtFather_or_spouce.Text = Patient.Father_or_Spouce;
            }
            else
            {
                Patient = new Domain.Patient();
                lblTitle.Content = "Create New Patient";
                btnCreateNewPatient.Content = "Create";
            }
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
