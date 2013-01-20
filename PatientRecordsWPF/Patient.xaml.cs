﻿using System;
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
    /// Interaction logic for Patient.xaml
    /// </summary>
    public partial class Patient : Page
    {
        MainWindow parent;
        public Patient(MainWindow objMainWindow)
        {
            InitializeComponent();
            parent = objMainWindow;
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            framePatientContainer.Content = new Visit();
        }

        private void btnPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            framePatientContainer.Content = new Details();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            parent.ShowSearch();
        }
    }
}