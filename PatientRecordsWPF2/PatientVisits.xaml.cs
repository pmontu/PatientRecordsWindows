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

        private void UpdateVisit()
        {
            mode = PatientVisits.Mode.update;
            btnCreateEditUpdateVisit.Content = "Update";
        }

        /* FRESH */
        private void CreateNewVisit()
        {
            /* HIDING add new visit button */
            btnAddNewVisit.Visibility = System.Windows.Visibility.Hidden;
            mode = PatientVisits.Mode.create;
            btnCreateEditUpdateVisit.Content = "Create";

            txtReferredBy.Text = "";
            txtDoctors_Email.Text = "";
            txtDoctor.Text = "";
            dtDate_of_Examination.Text = "";
            txtSymptom.Text = "";
            lbxSymptoms.ItemsSource = null;
            txtDiagnosis.Text = "";
            txtTreatment.Text = "";
            txtTag.Text = "";
            lbxTags.ItemsSource = null;

            TempVisitSymptoms = new List<Domain.Symptom>();
            TempVisitTags = new List<Domain.Tag>();
            
        }

        private void CreateNewVisitComplete()
        {
            /* SHOWing add new visit button */
            btnAddNewVisit.Visibility = System.Windows.Visibility.Visible;
        }

        private void wVisit_Activated(object sender, EventArgs e)
        {
            Start();
        }

        /* SAVE OF UPDATE */
        private void btnCreateEditUpdateVisit_Click(object sender, RoutedEventArgs e)
        {
            // VALIDATION
            bool isError = false;
            if (String.IsNullOrEmpty(txtDoctor.Text))
            {
                txtDoctor.BorderBrush = new BrushConverter().ConvertFromString("Red") as Brush;
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
            Visit.ReferredBy = txtReferredBy.Text;
            Visit.Doctors_Email = txtDoctors_Email.Text;
            Visit.Doctor = txtDoctor.Text;
            Visit.Date_of_Examination = dtDate_of_Examination.SelectedDate.Value;

            Visit.Diagnosis = txtDiagnosis.Text;
            Visit.Treatment = txtTreatment.Text;

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
                    UpdateVisit();
                }

                SelectedVisit = (Domain.Visit)lbxVisits.SelectedItem;

                txtReferredBy.Text = SelectedVisit.ReferredBy;
                txtDoctors_Email.Text = SelectedVisit.Doctors_Email;
                txtDoctor.Text = SelectedVisit.Doctor;
                dtDate_of_Examination.Text = SelectedVisit.Date_of_Examination.Date.ToString();

                txtDiagnosis.Text = SelectedVisit.Diagnosis;
                txtTreatment.Text = SelectedVisit.Treatment;

                TempVisitSymptoms = SelectedVisit.Symptoms.ToList();
                TempVisitTags = SelectedVisit.Tags.ToList();

                lbxSymptoms.ItemsSource = TempVisitSymptoms;
                lbxTags.ItemsSource = TempVisitTags;

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
        private void txtDoctor_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtDoctor.BorderBrush = new BrushConverter().ConvertFromString("#FFABADB3") as Brush;
        }

        private void dtDate_of_Examination_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtDate_of_Examination.BorderBrush = new BrushConverter().ConvertFromString("#FFABADB3") as Brush;
        }

        private void btnAddSymptom_Click(object sender, RoutedEventArgs e)
        {
            if (txtSymptom.Text != "")
            {
                Domain.Symptom symptom = new Domain.Symptom();
                symptom.Name = txtSymptom.Text;
                TempVisitSymptoms.Add(symptom);

                lbxSymptoms.ItemsSource = null;
                lbxSymptoms.ItemsSource = TempVisitSymptoms;
                txtSymptom.Text = "";
            }
        }

        private void btnAddTag_Click(object sender, RoutedEventArgs e)
        {
            if (txtTag.Text != "")
            {
                Domain.Tag tag = new Domain.Tag();
                tag.Name = txtTag.Text;
                TempVisitTags.Add(tag);

                lbxTags.ItemsSource = null;
                lbxTags.ItemsSource = TempVisitTags;
                txtTag.Text = "";
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

    }
}
