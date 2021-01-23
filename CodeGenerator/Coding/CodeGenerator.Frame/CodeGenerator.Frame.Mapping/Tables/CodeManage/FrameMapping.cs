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
    /// 数据库表映射--框架表
    /// </summary>
    public class FrameMapping : EntityTypeConfiguration<FrameEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrameMapping()
        {
            this.ToTable("Code_Frame");
            this.HasKey(x => x.Oid);
            this.Property(x => x.Code).HasColumnName("Frame_Code");
            this.Property(x => x.Name).HasColumnName("Frame_Name");
            this.Property(x => x.CreateUser).HasColumnName("Frame_CreateUser");
        }
    }
}
