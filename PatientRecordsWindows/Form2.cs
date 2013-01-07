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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            SQLiteDataAdapter da = new SQLiteDataAdapter("select name from patients", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            listBox1.DataSource = ds.Tables[0];
            listBox1.DisplayMember = "name";
        }
    }
}
