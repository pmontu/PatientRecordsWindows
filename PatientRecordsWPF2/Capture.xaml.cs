using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for Capture.xaml
    /// </summary>
    public partial class Capture : Window
    {
        public WebCam webcam { get; set; }

        public Capture()
        {
            InitializeComponent();
            webcam = new WebCam();
        }
        public Capture(EncoderDevice d)
            : this()
        {
            webcam.SelectedVideoDevice = d;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            webcam.StartRecording();
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            btnSnapshot.IsEnabled = false;

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (webcam.isRecording)
            {
                webcam.StopRecording();
                btnStop.IsEnabled = false;
                btnStart.IsEnabled = true;
                btnSnapshot.IsEnabled = true;
            }
        }

        private void btnSnapshot_Click(object sender, RoutedEventArgs e)
        {
            var md = new MediaDetails();
            md.Owner = this;
            md.ShowDialog();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                webcam.StartWebcam();
                webcam.LiveDeviceSource.PreviewWindow =
                new PreviewWindow(new HandleRef(WebcamPanel, WebcamPanel.Handle));
                btnStart.IsEnabled = true;
                btnSnapshot.IsEnabled = true;
            }
            catch
            {
                this.Close();
            }
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (webcam.isRecording)
                webcam.StopRecording();
            if (webcam.isConnected)
                webcam.StopWebcam();
        }

    }
}
