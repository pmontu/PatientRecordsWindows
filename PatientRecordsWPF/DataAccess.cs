using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWPF
{
    public static class DataAccess
    {
        public static ISession sess;
        public static void init(bool createdb=false)
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Domain.Patient).Assembly);

            // Get ourselves an NHibernate Session
            var sessions = cfg.BuildSessionFactory();
            sess = sessions.OpenSession();

            if (createdb)
            {
                new SchemaExport(cfg).Create(true, true);
            }
        }
    }
}
