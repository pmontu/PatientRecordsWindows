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
    public partial class PatientAdd : Form
    {
        public PatientAdd()
        {
            InitializeComponent();
        }

        private void btnPatientAddSave_Click(object sender, EventArgs e)
        {

            //validation
            string errors = "";
            string gender = rbM.Checked ? "M" : "F";
            int age;
            if (!Int32.TryParse(txtAge.Text, out age))
            {
                errors += "Age takes numeric only\n";
            }
            if (errors != "")
            {
                MessageBox.Show(errors);
                return;
            }

            var sess = ((Container)this.ParentForm).sess;
            var patient = new Domain.Patient
            {
                name = txtName.Text,
                age = Convert.ToInt32(txtAge.Text),
                gender = gender
            };

            sess.Save(patient);
            sess.Flush();

        }
    }
}
