using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Framework.Entity;

namespace Tests.Framework.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class CityMapping : EntityTypeConfiguration<CityEntity>
    {
        public CityMapping()
        {
            this.ToTable("tblcities");
            this.HasKey(x => x.Id);
            this.Property(x => x.Id).HasColumnName("City_Id");
            this.Property(x => x.Name).HasColumnName("CityName");
        }
    }
}
