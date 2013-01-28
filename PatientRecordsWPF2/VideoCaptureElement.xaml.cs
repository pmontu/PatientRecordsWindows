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

namespace PatientRecordsWPF2
{
    /// <summary>
    /// Interaction logic for VideoCaptureElement.xaml
    /// </summary>
    public partial class VideoCaptureElement : Page
    {
        public VideoCaptureElement()
        {
            InitializeComponent();
        }
        public VideoCaptureElement(string name)
            : this()
        {
            videoCapElement.VideoCaptureSource = name;
            videoCapElement.Play();
        }
    }
}
