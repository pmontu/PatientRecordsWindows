using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatientRecordsWindows
{
    public partial class PatientSearch : Form
    {
        public PatientSearch()
        {
            InitializeComponent();
        }

        private void PatientSearch_Activated(object sender, EventArgs e)
        {
            var sess = ((Container)this.ParentForm).sess;
            IQuery q = sess.CreateQuery("FROM Patient");
            var patients = q.List<Domain.Patient>();

            listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged;
            listBox1.DataSource = patients;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "Id";
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((Container)this.ParentForm).ShowPatientDetails(listBox1.SelectedValue.ToString());
        }
    }
}
