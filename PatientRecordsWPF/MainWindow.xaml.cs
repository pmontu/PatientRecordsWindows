using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
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

namespace PatientRecordsWPF
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

        private void PageSwitcher_Activated(object sender, EventArgs e)
        {
            ShowSearch();
            
            /*var sess = DataAccess.sess;

            var patient = new Domain.Patient
            {
                name = "test2"
            };

            sess.Save(patient);
            sess.Flush();*/
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ShowSearch();
        }


        public void ShowAdd()
        {
            frameContainer.Content = new Patient(this);

        }
        public void ShowSearch()
        {
            frameContainer.Content = new Search(this);

        }
    }
}
