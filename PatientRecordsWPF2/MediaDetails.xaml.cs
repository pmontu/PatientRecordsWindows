﻿using System;
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
        private bool isNew;
        private Domain.Medium Medium;

        public MediaDetails()
        {
            InitializeComponent();
        }
        public MediaDetails(bool isVideo)
            : this()
        {
            this.isVideo = isVideo;
            isNew = true;
        }
        public MediaDetails(Domain.Medium Medium)
            : this()
        {
            this.Medium = Medium;
            txtName.Text = Medium.Title;
            txtDescription.Text = Medium.Description;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            /* encapsulate details into Domain.Medium */
            string filename = "Media\\" + DateTime.Now.ToString("yyyyMMMdd-HHmmss-ddd");

            Domain.Medium medium;
            if (isNew)
            {
                medium = new Domain.Medium();
                if (isVideo)
                {
                    medium.Type = Domain.MediumType.Video;
                    filename += ".wmv";
                }
                else
                {
                    medium.Type = Domain.MediumType.Image;
                    filename += ".jpg";
                }

                medium.Path = filename;
            }
            else
                medium = Medium;

            File.Move(Directory.GetCurrentDirectory() + "\\medi.um", Directory.GetCurrentDirectory() + "\\" + filename);

            medium.Title = txtName.Text;
            medium.Description = txtDescription.Text;

            /* communicate with patientVisit to add medium to TempVisitMedium*/
            ((PatientVisits)this.Owner).AddMedium(medium);
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtName.Text = DateTime.Now.ToString("yyyyMMMdd-HHmmss-ddd");
        }
    }
}
