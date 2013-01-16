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
            frameContainer.Content = new Search();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            frameContainer.Content = new Search();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            frameContainer.Content = new Patient();
        }
    }
}
