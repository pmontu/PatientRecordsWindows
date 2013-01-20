using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWPF2.Domain
{
    public class Visit
    {
        public virtual int Id { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual string ReferredBy { get; set; }
        public virtual string Doctors_Email { get; set; }
        public virtual string Doctor { get; set; }
        public virtual DateTime Date_of_Examination { get; set; }
        public virtual IList<Symptom> Symptoms { get; set; }
        public virtual string Diagnosis { get; set; }
        public virtual string Treatment { get; set; }
        public virtual IList<Tag> Tags { get; set; }

        public Visit()
        {
            Symptoms = new List<Symptom>();
            Tags = new List<Tag>();
        }
    }

    public class VisitMap : ClassMap<Visit>
    {
        public VisitMap()
        {
            Id(x => x.Id);
            References(x => x.Patient);
            Map(x => x.ReferredBy).Nullable();
            Map(x => x.Doctors_Email).Nullable();
            Map(x => x.Doctor).Not.Nullable();
            Map(x => x.Date_of_Examination).Not.Nullable();
            HasManyToMany(x => x.Symptoms)
             .Cascade.All()
             .Table("VisitSymptom");
            Map(x => x.Diagnosis).Nullable();
            Map(x => x.Treatment).Nullable();
            HasManyToMany(x => x.Tags)
             .Cascade.All()
             .Table("VisitTag");
            Table("Visit");
        }
    }

    public class Symptom
    {
        public virtual int Id { get; set; }
        public virtual IList<Visit> VisitsIn { get; set; }
        public virtual string Name { get; set; }

        public Symptom()
        {
            VisitsIn = new List<Visit>();
        }
    }
    public class SymptomMap : ClassMap<Symptom>
    {
        public SymptomMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable();
            HasManyToMany(x => x.VisitsIn)
             .Cascade.All()
             .Table("VisitSymptom");
        }
    }
    public class Tag
    {
        public virtual int Id { get; set; }
        public virtual IList<Visit> VisitsIn { get; set; }
        public virtual string Name { get; set; }

        public Tag()
        {
            VisitsIn = new List<Visit>();
        }

    }
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Id(x => x.Id);
            Map(x => x.Name).Not.Nullable();
            HasManyToMany(x => x.VisitsIn)
             .Cascade.All()
             .Table("VisitTag");
        }
    }
}
