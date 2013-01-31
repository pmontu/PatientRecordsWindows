using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
                ((PatientVisits)this.Owner).ShowMediaDetails(true);
            }
        }

        private void btnSnapshot_Click(object sender, RoutedEventArgs e)
        {
            // Create the bitmap image
            Bitmap bitmap = new Bitmap(WebcamPanel.Width, WebcamPanel.Height);
            // Create the graphics object that will copy from the screen into the bitmap image
            Graphics graphics = Graphics.FromImage(bitmap);
            // Create a rectangle egal to the size of the previewVideoPanel
            System.Drawing.Rectangle rectangleVideoPreview = WebcamPanel.Bounds;
            // Create a point egal to the origin location (upper left point) of the previewVideoPanel
            System.Drawing.Point sourcesPoint =
                WebcamPanel.PointToScreen(new System.Drawing.Point(WebcamPanel.ClientRectangle.X,
                                                     WebcamPanel.ClientRectangle.Y));
            // Copy the content of this rectangle into the bitmap image using the object graphics
            graphics.CopyFromScreen(sourcesPoint, System.Drawing.Point.Empty, rectangleVideoPreview.Size);
            // Define the path for save the file
            String completeImagePath = Directory.GetCurrentDirectory() + "\\medi.um";
            // Save the image 
            bitmap.Save(completeImagePath, ImageFormat.Jpeg);
            
            /* communicate to PatientVisits to show next window - collect details of image */
            ((PatientVisits)this.Owner).ShowMediaDetails(false);
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
