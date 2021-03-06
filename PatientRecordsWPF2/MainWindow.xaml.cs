﻿using NHibernate;
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
using System.Data.SQLite;
using NHibernate.Criterion;
using System.IO;

namespace PatientRecordsWPF2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ISession session { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            session = ((App)Application.Current).session;
        }

        /* VIEW PATIENT VISITS */

        private void btnViewPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            ViewPatientVisits();
        }

        private void ViewPatientVisits()
        {
            if (lbxPatients.SelectedIndex != -1)
            {
                var pv = new PatientVisits((Domain.Patient)lbxPatients.SelectedItem);
                pv.Owner = this;
                pv.ShowDialog();
            }
        }

        /* ADD NEW PATIENT */
        private void btnAddNewPatient_Click(object sender, RoutedEventArgs e)
        {
            var pd = new PatientDetails();
            pd.Owner = this;
            pd.ShowDialog();
            load();
        }

        /* PAGE INIT */
        private void wSearch_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }

        private void load()
        {
            var patients = session.CreateCriteria<Domain.Patient>().List<Domain.Patient>();

            lbxPatients.ItemsSource = patients;

            /* keyboard focus on search filter textbox */
            Keyboard.Focus(txtSearchFilter);   
        }
        
        /* SEARCH FILTER */
        private void txtSearchFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbxPatients.Items.Filter = a =>
            {
                if (((Domain.Patient)a).Name.ToString().ToLower().Contains(txtSearchFilter.Text.ToLower()))
                {
                    return true;
                }
                return false;
            };
        }

        private void lbxPatients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewPatientVisits();
        }


        internal void cleanup()
        {
            var CleanupMedia = session.CreateCriteria<Domain.Cleanup>().List();
            bool isFileDeleted;
            foreach (Domain.Cleanup c in CleanupMedia)
            {
                var p = c.Path;
                isFileDeleted = true;
                string f = Directory.GetCurrentDirectory() + @"\" + p;
                if (p.Contains(".wmv"))
                {
                    var t = f + @"-thumbnail.jpg";
                    if (File.Exists(t))
                    {
                        try
                        {
                            File.Delete(t);
                        }
                        catch
                        {
                            isFileDeleted = false;
                        }
                    }
                }
                if (File.Exists(f))
                {
                    try
                    {
                        File.Delete(f);
                    }
                    catch
                    {
                        isFileDeleted = false;
                    }
                }
                if (isFileDeleted)
                {
                    session.Delete(c);
                    session.Flush();
                }
            }
        }

        private void wSearch_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cleanup();
        }

        private void btnAdvancedSearch_Click(object sender, RoutedEventArgs e)
        {
            /* show analysis window dialog */
            var wAnalysis = new Analysis();
            wAnalysis.Owner = this;
            wAnalysis.ShowDialog();
        }
    }
}
