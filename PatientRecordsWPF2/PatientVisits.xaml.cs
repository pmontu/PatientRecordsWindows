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
using NHibernate.Criterion;
using NHibernate;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using System.Runtime.InteropServices;

namespace PatientRecordsWPF2
{
    /// <summary>
    /// Interaction logic for PatientVisits.xaml
    /// 
    /// opetations - 
    /// 1. create visit 
    ///     1. if no visits allreay present for patient
    ///         1. create visit
    ///             1. validate
    ///     2. if add new visit has been clicked if visits allready present for patient
    ///         1. jump to another visit
    ///         2. create visit
    ///             1. validate
    /// 2. populate latest visits if visits allready present for patient
    ///     1. edit and update visit
    ///         1.validate
    ///     2. jump to add new visit
    /// </summary>
    public partial class PatientVisits : Window
    {
        private Domain.Patient Patient;
        private enum Mode { create, update };
        private Mode mode;
        private Domain.Visit SelectedVisit;
        private List<Domain.Symptom> TempVisitSymptoms;
        private List<Domain.Tag> TempVisitTags;
        private WebCam webcam;

        public PatientVisits()
        {
            InitializeComponent();
        }
        public PatientVisits(Domain.Patient Patient) : this()
        {
            this.Patient = Patient;
        }

        private void Start()
        {
            var session = ((App)Application.Current).session;

            /* though patient object has been passed 
                * to access visits the session has to be
                * initialised again */
            Patient = session.Get<Domain.Patient>(Patient.Id);

            Refresh();

            lblTitle.Content = Patient.Name;
            if (Patient.Visits.Count == 0)
            {
                CreateNewVisit();
            }
            else
            {
                /* Sort Visit List by desc date */
                lbxVisits.ItemsSource = Patient.Visits.OrderByDescending(x => x.Date_of_Examination);
                    
                /* select latest visit - fires event */
                lbxVisits.SelectedIndex = 0;

                UpdateVisit();

            }
        }

        private void Refresh()
        {
            /* Auto complete fields */
            acbDoctor.ItemsSource = DataBindAutoCompleteField("Doctor", typeof(Domain.Visit));
            acbReferredBy.ItemsSource = DataBindAutoCompleteField("ReferredBy", typeof(Domain.Visit));
            acbDoctors_Email.ItemsSource = DataBindAutoCompleteField("Doctors_Email", typeof(Domain.Visit));
            acbSymptom.ItemsSource = DataBindAutoCompleteField("Name", typeof(Domain.Symptom));
            acbDiagnosis.ItemsSource = DataBindAutoCompleteField("Diagnosis", typeof(Domain.Visit));
            acbTreatment.ItemsSource = DataBindAutoCompleteField("Treatment", typeof(Domain.Visit));
            acbTag.ItemsSource = DataBindAutoCompleteField("Name", typeof(Domain.Tag));
        }

        private System.Collections.IEnumerable DataBindAutoCompleteField(string Column, Type classtype)
        {
            var session = ((App)Application.Current).session;

            return session.CreateCriteria(classtype)
                .SetProjection(Projections.Distinct(Projections.ProjectionList().Add(Projections.Alias(Projections.Property(Column), Column))))
                //.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(Domain.Visit)))
                .List();
        }

        private void UpdateVisit()
        {
            mode = PatientVisits.Mode.update;
            btnCreateEditUpdateVisit.Content = "Update";
            btnCreateEditUpdateVisit.IsEnabled = false;
        }

        /* FRESH */
        private void CreateNewVisit()
        {
            /* HIDING add new visit button */
            btnAddNewVisit.Visibility = System.Windows.Visibility.Hidden;
            mode = PatientVisits.Mode.create;
            btnCreateEditUpdateVisit.Content = "Create";

            acbReferredBy.Text = "";
            acbDoctors_Email.Text = "";
            acbDoctor.Text = "";
            dtDate_of_Examination.Text = "";
            acbSymptom.Text = "";
            lbxSymptoms.ItemsSource = null;
            acbDiagnosis.Text = "";
            acbTreatment.Text = "";
            acbTag.Text = "";
            lbxTags.ItemsSource = null;

            TempVisitSymptoms = new List<Domain.Symptom>();
            TempVisitTags = new List<Domain.Tag>();
            
        }

        private void CreateNewVisitComplete()
        {
            /* SHOWing add new visit button */
            btnAddNewVisit.Visibility = System.Windows.Visibility.Visible;
        }

        private void wVisit_Loaded(object sender, RoutedEventArgs e)
        {
            Start();

        }

        /* SAVE OF UPDATE */
        private void btnCreateEditUpdateVisit_Click(object sender, RoutedEventArgs e)
        {
            // VALIDATION
            bool isError = false;
            if (String.IsNullOrEmpty(acbDoctor.Text))
            {
                acbDoctor.BorderBrush = new BrushConverter().ConvertFromString("Red") as Brush;
                isError = true;
            }
            if (!dtDate_of_Examination.SelectedDate.HasValue)
            {
                dtDate_of_Examination.SetValue(Border.BorderBrushProperty, Brushes.Red);
                isError = true;
            }
            if (isError)
            {
                return;
            }

            Domain.Visit Visit;
            if (mode == Mode.create)
            {
                Visit = new Domain.Visit();
                Visit.Patient = Patient;
                Patient.Visits.Add(Visit);
            }
            else
            {
                Visit = SelectedVisit;
            }

            // ENCAPSULATION
            Visit.ReferredBy = acbReferredBy.Text;
            Visit.Doctors_Email = acbDoctors_Email.Text;
            Visit.Doctor = acbDoctor.Text;
            Visit.Date_of_Examination = dtDate_of_Examination.SelectedDate.Value;

            Visit.Diagnosis = acbDiagnosis.Text;
            Visit.Treatment = acbTreatment.Text;

            foreach (Domain.Symptom sym in TempVisitSymptoms)
            {
                bool isNew = true;
                foreach (Domain.Symptom s in Visit.Symptoms)
                {
                    if (s.Name == sym.Name)
                    {
                        isNew = false;
                    }
                }
                if (isNew)
                {
                    sym.Visit = Visit;
                    Visit.Symptoms.Add(sym);
                }
            }
            foreach (Domain.Tag tag in TempVisitTags)
            {
                bool isNew = true;
                foreach (Domain.Tag t in Visit.Tags)
                {
                    if (t.Name == tag.Name)
                    {
                        isNew = false;
                    }
                }
                if (isNew)
                {
                    tag.Visit = Visit;
                    Visit.Tags.Add(tag);
                }
            }

            List<Domain.Symptom> toberemovedSymptoms = new List<Domain.Symptom>();
            foreach (Domain.Symptom s in Visit.Symptoms)
            {
                bool isPresent = false;
                foreach (Domain.Symptom sym in TempVisitSymptoms)
                {
                    if (s.Name == sym.Name)
                    {
                        isPresent = true;
                    }
                }
                if (!isPresent)
                {
                    toberemovedSymptoms.Add(s);
                }
            }
            foreach (Domain.Symptom s in toberemovedSymptoms)
            {
                s.Visit = null;
                Visit.Symptoms.Remove(s);
            }

            List<Domain.Tag> toberemovedTags = new List<Domain.Tag>();
            foreach (Domain.Tag t in Visit.Tags)
            {
                bool isPresent = false;
                foreach (Domain.Tag tag in TempVisitTags)
                {
                    if (t.Name == tag.Name)
                    {
                        isPresent = true;
                    }
                }
                if (!isPresent)
                {
                    toberemovedTags.Add(t);
                }
            }
            foreach (Domain.Tag t in toberemovedTags)
            {
                t.Visit = null;
                Visit.Tags.Remove(t);
            }


            // DB
            var session = ((App)Application.Current).session;

            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(Visit);
                transaction.Commit();
            }

            if (mode == Mode.create)
            {
                CreateNewVisitComplete();
                Start();
            }
            else
            {
                UpdateVisit();
                Refresh();
            }

        }

        /* populates selected visit details */        
        private void lbxVisits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbxVisits.SelectedIndex != -1)
            {
                /* If User clicks on previous visits while creating a visit 
                 * ui needs to be updated for updating */
                if (mode == Mode.create)
                {
                    CreateNewVisitComplete();
                }
                
                SelectedVisit = (Domain.Visit)lbxVisits.SelectedItem;

                acbReferredBy.Text = SelectedVisit.ReferredBy;
                acbDoctors_Email.Text = SelectedVisit.Doctors_Email;
                acbDoctor.Text = SelectedVisit.Doctor;
                dtDate_of_Examination.Text = SelectedVisit.Date_of_Examination.Date.ToString();

                acbDiagnosis.Text = SelectedVisit.Diagnosis;
                acbTreatment.Text = SelectedVisit.Treatment;

                TempVisitSymptoms = SelectedVisit.Symptoms.ToList();
                TempVisitTags = SelectedVisit.Tags.ToList();

                lbxSymptoms.ItemsSource = TempVisitSymptoms;
                lbxTags.ItemsSource = TempVisitTags;
                acbSymptom.Text = "";
                acbTag.Text = "";
                
                UpdateVisit();

            }
        }

        private void btnAddNewVisit_Click(object sender, RoutedEventArgs e)
        {
            lbxVisits.SelectedIndex = -1;
            CreateNewVisit();
        }

        /* OPEN DETAILS OF A PATIENT */
        private void lblTitle_Click(object sender, RoutedEventArgs e)
        {
            var pd = new PatientDetails(Patient);
            pd.Owner = this;
            pd.ShowDialog();
        }

        /* VALIDATION COLOR CHANGES BACK TO NORMAL */
        private void acbDoctor_TextChanged(object sender, RoutedEventArgs e)
        {
            acbDoctor.BorderBrush = new BrushConverter().ConvertFromString("#FFABADB3") as Brush;
            SomeChangeForUpdate();
        }
        private void dtDate_of_Examination_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtDate_of_Examination.BorderBrush = new BrushConverter().ConvertFromString("#FFABADB3") as Brush;
            SomeChangeForUpdate();
        }

        private void btnAddSymptom_Click(object sender, RoutedEventArgs e)
        {
            if (acbSymptom.Text != "")
            {
                Domain.Symptom symptom = new Domain.Symptom();
                symptom.Name = acbSymptom.Text;
                TempVisitSymptoms.Add(symptom);

                lbxSymptoms.ItemsSource = null;
                lbxSymptoms.ItemsSource = TempVisitSymptoms;
                acbSymptom.Text = "";
            }
        }

        private void btnAddTag_Click(object sender, RoutedEventArgs e)
        {
            if (acbTag.Text != "")
            {
                Domain.Tag tag = new Domain.Tag();
                tag.Name = acbTag.Text;
                TempVisitTags.Add(tag);

                lbxTags.ItemsSource = null;
                lbxTags.ItemsSource = TempVisitTags;
                acbTag.Text = "";
            }
        }

        private void btnRemoveSymptom_Click(object sender, RoutedEventArgs e)
        {
            TempVisitSymptoms.Remove(((Domain.Symptom)((Button)sender).DataContext));
            lbxSymptoms.ItemsSource = null;
            lbxSymptoms.ItemsSource = TempVisitSymptoms;
        }

        private void btnRemoveTag_Click(object sender, RoutedEventArgs e)
        {
            TempVisitTags.Remove(((Domain.Tag)((Button)sender).DataContext));
            lbxTags.ItemsSource = null;
            lbxTags.ItemsSource = TempVisitTags;

        }

        private void SomeChangeForUpdate()
        {
            btnCreateEditUpdateVisit.IsEnabled = true;
        }        

        private void acbReferredBy_TextChanged(object sender, RoutedEventArgs e)
        {
            SomeChangeForUpdate();
        }

        private void acbDoctors_Email_TextChanged(object sender, RoutedEventArgs e)
        {
            SomeChangeForUpdate();
        }

        private void lbxSymptoms_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            SomeChangeForUpdate();
        }

        private void acbDiagnosis_TextChanged(object sender, RoutedEventArgs e)
        {
            SomeChangeForUpdate();
        }

        private void acbTreatment_TextChanged(object sender, RoutedEventArgs e)
        {
            SomeChangeForUpdate();
        }

        private void lbxTags_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            SomeChangeForUpdate();
        }

        private void cbxImageDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxImageDevices.SelectedIndex != -1)
            {
                if(webcam.isConnected)
                    webcam.StopWebcam();
                webcam.SelectedVideoDevice = (EncoderDevice)cbxImageDevices.SelectedItem;
                webcam.StartWebcam();
                webcam.LiveDeviceSource.PreviewWindow =
                new PreviewWindow(new HandleRef(WebcamPanel, WebcamPanel.Handle));
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

        private void tabVisit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != tabVisit )
                return;

            if ((TabItem)tabVisit.SelectedItem == tabitemPhotos)
            {
                btnStart.IsEnabled = false;
                btnStop.IsEnabled = false;
                btnSnapshot.IsEnabled = false;

                if (webcam == null)
                {
                    webcam = new WebCam(); 
                    
                    if (webcam.InitializeListVideoDevices() > 0)
                    {
                        cbxImageDevices.ItemsSource = null;
                        cbxImageDevices.ItemsSource = webcam.VideoDevices;
                        cbxImageDevices.DisplayMemberPath = "Name";
                    }
                }
                

            }
            else
            {
                if (webcam != null)
                    if (webcam.isConnected)
                    {
                        webcam.StopWebcam();
                        cbxImageDevices.SelectedIndex = -1;
                    }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (webcam != null)
                if (webcam.isConnected)
                {
                    webcam.StartRecording();
                    btnStart.IsEnabled = false;
                    btnStop.IsEnabled = true;
                    btnSnapshot.IsEnabled = false;
                }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (webcam != null)
                if (webcam.isRecording)
                {
                    webcam.StopRecording();
                    btnStop.IsEnabled = false;
                    btnStart.IsEnabled = true;
                    btnSnapshot.IsEnabled = true;
                }
        }
    }

    public class WebCam
    {
        public List<EncoderDevice> VideoDevices {get; set;}
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
            //System.Drawing.Size framesize = new System.Drawing.Size(1280, 960);
            //LiveDeviceSource.PickBestVideoFormat(framesize, 30);
            //LiveJob.OutputFormat.VideoProfile.Size = framesize;
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
