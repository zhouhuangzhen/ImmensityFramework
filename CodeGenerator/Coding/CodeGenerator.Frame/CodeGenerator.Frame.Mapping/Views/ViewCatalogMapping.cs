using CodeGenerator.Frame.Entity.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Mapping.Views
{
    /// <summary>
    /// 数据库视图映射--字典视图
    /// </summary>
    public class ViewCatalogMapping:EntityTypeConfiguration<ViewCatalogEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViewCatalogMapping()
        {
            this.ToTable("V_Catalog");
            this.HasKey(x => x.Oid);
            this.Property(x => x.ItemCode).HasColumnName("Cld_Code");
            this.Property(x => x.ItemName).HasColumnName("Cld_Name");
            this.Property(x => x.ItemValue).HasColumnName("Cld_Value");
            this.Property(x => x.ItemText).HasColumnName("Cld_Text");
            this.Property(x => x.ItemOrder).HasColumnName("Cld_Order"); 
            this.Property(x => x.ItemParentOid).HasColumnName("Cld_ParentOid");
            this.Property(x => x.CatalogCode).HasColumnName("ClCode");
            this.Property(x => x.CatalogName).HasColumnName("ClName");
            this.Property(x => x.CatalogType).HasColumnName("ClType");
            this.Property(x => x.CatalogText).HasColumnName("ClText");
        }
    }
}
