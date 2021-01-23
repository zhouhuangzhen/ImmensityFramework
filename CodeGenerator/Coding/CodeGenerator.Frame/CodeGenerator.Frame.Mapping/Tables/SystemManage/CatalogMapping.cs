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
    /// 数据库表映射--字典分类表
    /// </summary>
    public class CatalogMapping : EntityTypeConfiguration<CatalogEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CatalogMapping()
        {
            this.ToTable("Code_Catalog");
            this.HasKey(x => x.Oid);
            this.Property(x => x.Code).HasColumnName("Cl_Code");
            this.Property(x => x.Name).HasColumnName("Cl_Name");
            this.Property(x => x.Type).HasColumnName("Cl_Type");
            this.Property(x => x.Text).HasColumnName("Cl_Text");
            this.Property(x => x.CreateUser).HasColumnName("Cl_CreateUser");
        }
    }
}
