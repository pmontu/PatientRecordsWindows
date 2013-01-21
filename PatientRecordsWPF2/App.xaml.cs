using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PatientRecordsWPF2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ISession session { get; set; }
        public App() 
        {
            var sessionFactory = CreateSessionFactory();
            session = sessionFactory.OpenSession();
            if (!session.IsConnected)
            {
                MessageBox.Show("Unable to connect to database, please make sure PR.db is present in the same directory as the application");
                Application.Current.Shutdown();
            }
        }
        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                SQLiteConfiguration.Standard
                .UsingFile("PR.db")
                )
                .Mappings(m =>
                            m.FluentMappings.AddFromAssemblyOf<App>())
                //.ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
              .Create(false, true);
        }
    }
}
