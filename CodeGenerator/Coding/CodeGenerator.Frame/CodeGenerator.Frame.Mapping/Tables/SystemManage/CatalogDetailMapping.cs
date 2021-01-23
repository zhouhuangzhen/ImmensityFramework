using CodeGenerator.Frame.Entity.Tables.SystemManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Mapping.Tables.SystemManage
{
    /// <summary>
    /// 数据库表映射--字典明细表
    /// </summary>
    public class CatalogDetailMapping : EntityTypeConfiguration<CatalogDetailEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CatalogDetailMapping()
        {
            this.ToTable("Code_Catalog_Detail");
            this.HasKey(x => x.Oid);
            this.Property(x => x.Code).HasColumnName("Cld_Code");
            this.Property(x => x.Name).HasColumnName("Cld_Name");
            this.Property(x => x.Value).HasColumnName("Cld_Value");
            this.Property(x => x.Text).HasColumnName("Cld_Text");
            this.Property(x => x.Order).HasColumnName("Cld_Order");
            this.Property(x => x.ParentOid).HasColumnName("Cld_ParentOid");
            this.Property(x => x.CreateUser).HasColumnName("Cld_CreateUser");
        }
    }
}
