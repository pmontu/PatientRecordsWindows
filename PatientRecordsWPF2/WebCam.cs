using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using Microsoft.Expression.Encoder.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWPF2
{
    public class WebCam
    {
        public List<EncoderDevice> VideoDevices { get; set; }
        public LiveJob LiveJob { get; set; }
        public LiveDeviceSource LiveDeviceSource { get; set; }
        public EncoderDevice SelectedVideoDevice { get; set; }
        public bool isConnected { get; set; }
        public bool isRecording { get; set; }
        public FileArchivePublishFormat FileArchivePublishFormat { get; set; }

        public WebCam()
        {
            VideoDevices = new List<EncoderDevice>();
            isConnected = false;
        }
        public int InitializeListVideoDevices()
        {
            int nb = 0;
            foreach (EncoderDevice encoderDevice in EncoderDevices.FindDevices(EncoderDeviceType.Video))
            {
                VideoDevices.Add(encoderDevice);
                nb++;
            }
            return nb;
        }
        public bool StartWebcam()
        {
            if (SelectedVideoDevice == null) return false;
            LiveJob = null;
            LiveJob = new LiveJob();
            LiveDeviceSource = LiveJob.AddDeviceSource(SelectedVideoDevice, null);
            System.Drawing.Size framesize = new System.Drawing.Size(1280, 960);
            LiveDeviceSource.PickBestVideoFormat(framesize, 30);
            LiveJob.OutputFormat.VideoProfile.Size = framesize;
            LiveJob.OutputFormat.VideoProfile.Bitrate = new ConstantBitrate(2000);
            LiveJob.ActivateSource(LiveDeviceSource);
            isConnected = true;
            return true;
        }
        public void StopWebcam()
        {
            LiveJob.RemoveDeviceSource(LiveDeviceSource);
            LiveDeviceSource = null;
            isConnected = false;
        }
        public bool StartRecording()
        {
            FileArchivePublishFormat = new FileArchivePublishFormat();
            FileArchivePublishFormat.OutputFileName = @"C:\Users\ManojKumar\Desktop\temp.wmv";
            LiveJob.PublishFormats.Add(FileArchivePublishFormat);
            LiveJob.StartEncoding();
            isRecording = true;
            return true;
        }
        public void StopRecording()
        {
            LiveJob.StopEncoding();
            isRecording = false;
        }
    }

}
