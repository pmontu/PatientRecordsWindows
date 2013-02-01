using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordsWPF2.Domain
{
    public enum MediumType
    {
        Video,
        Image
    };
    public class Medium
    {
        public virtual int Id { get; set; }
        public virtual Visit Visit { get; set; }
        public virtual MediumType Type { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Path { get; set; }
    }

    public class MediumMap : ClassMap<Medium>
    {
        public MediumMap()
        {
            Id(x => x.Id);
            References(x => x.Visit);
            Map(x => x.Type).Not.Nullable();
            Map(x => x.Title).Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.Path).Not.Nullable();
        }
    }

    public class Cleanup
    {
        public virtual int Id { get; set; }
        public virtual string Path { get; set; }
    }

    public class CleanupMap : ClassMap<Cleanup>
    {
        public CleanupMap()
        {
            Id(x => x.Id);
            Map(x => x.Path).Not.Nullable();
        }
    }
}
