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
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Page
    {
        MainWindow parent;
        public Search(MainWindow objMainWindow)
        {
            InitializeComponent();
            parent = objMainWindow;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            parent.ShowAdd();
        }
    }
}
