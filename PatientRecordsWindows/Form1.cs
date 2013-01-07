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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SQLiteConnection.CreateFile("MyDatabase.sqlite");

            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            /*string sql = "CREATE TABLE patients (name TEXT, age INTEGER, gender TEXT)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();*/

            string sql2 = "insert into patients (name, age, gender) values ($name, $age, $gender)";
            SQLiteCommand command2 = new SQLiteCommand(sql2, m_dbConnection);
            command2.Parameters.AddWithValue("$name", txtName.Text);
            command2.Parameters.AddWithValue("$age", txtAge.Text);
            string gender = rbM.Checked ? "M" : "F";
            command2.Parameters.AddWithValue("$gender", gender);
            command2.ExecuteNonQuery();

        }
    }
}
