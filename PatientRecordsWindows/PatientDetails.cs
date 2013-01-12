using NHibernate;
using NHibernate.Criterion;
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
    public partial class PatientDetails : Form
    {
        private String PatientID;

        public PatientDetails(String PatientID)
        {
            InitializeComponent();
            this.PatientID = PatientID;
        }

        private void PatientDetails_Activated(object sender, EventArgs e)
        {
            var sess = ((Container)this.ParentForm).sess;
            Guid guid;
            Guid.TryParse(this.PatientID,out guid);
            var patient = GetUserByUserGuid(guid);
            lblPatientName.Text = patient.name;
            lblPatientAge.Text = patient.age.ToString();
            lblPatientGender.Text = patient.gender;

        }
        public Domain.Patient GetUserByUserGuid(Guid patientGuid)
        {
            var sess = ((Container)this.ParentForm).sess;
            ICriteria crit = sess.CreateCriteria<Domain.Patient>();
            crit.Add(Expression.Eq("Id", patientGuid));
            return crit.UniqueResult<Domain.Patient>(); //will return null if not found
        }
    }
}
