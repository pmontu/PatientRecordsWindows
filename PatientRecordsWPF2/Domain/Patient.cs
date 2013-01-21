using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWPF2.Domain
{
    public enum Sex
    {
        Male,
        Female
    };
    public class Patient
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Father_or_Spouce { get; set; }
        public virtual Sex Sex { get; set; }
        public virtual DateTime Date_of_Birth { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Pin { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Email { get; set; }
        public virtual IList<Visit> Visits { get; set; }

        public virtual void AddVisit(Visit visit)
        {
            visit.Patient = this;
            Visits.Add(visit);
        }
    }

    public class PatientMap : ClassMap<Patient>
    {
        public PatientMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Father_or_Spouce).Nullable();
            Map(x => x.Sex).Not.Nullable();
            Map(x => x.Date_of_Birth).Nullable();
            Map(x => x.Address).Nullable();
            Map(x => x.City).Nullable();
            Map(x => x.State).Nullable();
            Map(x => x.Pin).Nullable();
            Map(x => x.Phone).Nullable();
            Map(x => x.Email).Nullable();
            HasMany(x => x.Visits)
             .Inverse()
             .Cascade.All();
        }
    }
}
