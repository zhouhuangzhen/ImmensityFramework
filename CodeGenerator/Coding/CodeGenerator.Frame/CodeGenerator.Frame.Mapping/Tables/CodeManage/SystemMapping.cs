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
    /// 数据库表映射--系统信息表
    /// </summary>
    public class SystemMapping : EntityTypeConfiguration<SystemEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemMapping()
        {
            this.ToTable("Code_System");
            this.HasKey(x => x.Oid);
            this.Property(x => x.Code).HasColumnName("Sys_Code");
            this.Property(x => x.Name).HasColumnName("Sys_Name");
            this.Property(x => x.IP).HasColumnName("Sys_IP");
            this.Property(x => x.Config).HasColumnName("Sys_Config");
            this.Property(x => x.Type).HasColumnName("Sys_Type");
            this.Property(x => x.Solution).HasColumnName("Sys_Solution");
            this.Property(x => x.Schema).HasColumnName("Sys_Schema");
            this.Property(x => x.CreateUser).HasColumnName("Sys_CreateUser");
        }
    }
}
