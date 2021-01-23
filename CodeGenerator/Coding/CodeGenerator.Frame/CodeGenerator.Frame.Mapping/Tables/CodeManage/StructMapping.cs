using CodeGenerator.Frame.Entity.Tables.CodeManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Mapping.Tables.CodeManage
{
    /// <summary>
    /// 数据库表映射--框架结构表
    /// </summary>
    public class StructMapping : EntityTypeConfiguration<StructEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StructMapping()
        {
            this.ToTable("Code_Struct");
            this.HasKey(x => x.Oid);
            this.Property(x => x.Code).HasColumnName("Struct_Code");
            this.Property(x => x.Name).HasColumnName("Struct_Name");
            this.Property(x => x.FrameId).HasColumnName("Struct_FrameId");
            this.Property(x => x.Order).HasColumnName("Struct_Order");
            this.Property(x => x.CreateUser).HasColumnName("Struct_CreateUser");
        }
    }
}
