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
    /// Interaction logic for PatientVisits.xaml
    /// </summary>
    public partial class PatientVisits : Window
    {
        private Domain.Patient Patient;

        public PatientVisits()
        {
            InitializeComponent();
        }
        public PatientVisits(Domain.Patient Patient) : this()
        {
            this.Patient = Patient;
        }

        private void wVisit_Activated(object sender, EventArgs e)
        {
            lblTitle.Content = Patient.Name;
        }

    }
}
