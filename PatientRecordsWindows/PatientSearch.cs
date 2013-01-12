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

        private void Form2_Activated(object sender, EventArgs e)
        {
            listBox1.SelectedIndexChanged -= listBox1_SelectedIndexChanged;
            SQLiteConnection con = new SQLiteConnection("Data Source=PR.sqlite;Version=3;");
            SQLiteDataAdapter da = new SQLiteDataAdapter("select Id,name from patient", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            listBox1.DataSource = ds.Tables[0];
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
