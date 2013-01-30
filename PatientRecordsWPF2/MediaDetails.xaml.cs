using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MediaDetails.xaml
    /// </summary>
    public partial class MediaDetails : Window
    {
        private bool isVideo;

        public MediaDetails()
        {
            InitializeComponent();
        }
        public MediaDetails(bool isVideo)
            : this()
        {
            this.isVideo = isVideo;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string filename = DateTime.Now.ToString("yyyyMMMdd-HHmmss-ddd") + ".medium";
            File.Move(Directory.GetCurrentDirectory() + "\\medi.um", Directory.GetCurrentDirectory() + "\\Media\\" + filename);

            Domain.Medium medium = new Domain.Medium();
            medium.Title = txtName.Text;
            medium.Description = txtDescription.Text;
            medium.Path = filename;
            if (isVideo)
                medium.Type = Domain.MediumType.Video;
            else
                medium.Type = Domain.MediumType.Image;
            ((PatientVisits)this.Owner).AddMedium(medium);
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtName.Text = DateTime.Now.ToString("yyyyMMMdd-HHmmss-ddd");
        }
    }
}
