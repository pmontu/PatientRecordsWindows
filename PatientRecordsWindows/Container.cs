using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatientRecordsWindows
{

    public partial class Container : Form
    {
        Configuration cfg;
        ISessionFactory sessions;
        public ISession sess;

        public Container()
        {
            InitializeComponent();
        }

        private void PatientAdd(object sender, EventArgs e)
        {
            Form childForm = new PatientAdd();
            childForm.MdiParent = this;
            childForm.Dock = DockStyle.Fill;
            childForm.Show();
        }

        private void PatientSearch(object sender, EventArgs e)
        {
            Form childForm = new PatientSearch();
            childForm.MdiParent = this;
            childForm.Dock = DockStyle.Fill;
            childForm.Show();
        }
        public void ShowPatientDetails(String patientid)
        {
            PatientDetails childForm = new PatientDetails(patientid);
            childForm.MdiParent = this;
            childForm.Dock = DockStyle.Fill;
            childForm.Show();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {

            //new SchemaExport(this.cfg).Create(true, true);

            // Create a Product...
            var patient = new Domain.Patient
            {
                name = "user 4",
                age = 50,
                gender = "M"
            };

            // And save it to the database
            this.sess.Save(patient);
            this.sess.Flush();

            IQuery q = this.sess.CreateQuery("FROM Patient");
            var list = q.List<Domain.Patient>();

            // List all the entries' names
            string str = "";
            list.ToList().ForEach(p => str += p.name);
            MessageBox.Show(str);
        }

        private void MDIParent1_Load(object sender, EventArgs e)
        {
            this.cfg = new Configuration();
            this.cfg.Configure();
            this.cfg.AddAssembly(typeof(Domain.Patient).Assembly);

            // Get ourselves an NHibernate Session
            this.sessions = this.cfg.BuildSessionFactory();
            this.sess = sessions.OpenSession();
        }
    }
}
