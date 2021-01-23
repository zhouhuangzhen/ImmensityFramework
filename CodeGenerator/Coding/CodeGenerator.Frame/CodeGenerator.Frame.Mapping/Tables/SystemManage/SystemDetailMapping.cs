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
    /// 数据库表映射--系统明细配置表
    /// </summary>
    public class SystemDetailMapping : EntityTypeConfiguration<SystemDetailEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemDetailMapping()
        {
            this.ToTable("Code_Detail_System");
            this.HasKey(x => x.Oid);
            this.Property(x => x.DetailId).HasColumnName("Ds_DetailId");
            this.Property(x => x.SystemId).HasColumnName("Ds_SystemId");
            this.Property(x => x.Value).HasColumnName("Ds_Value");
            this.Property(x => x.CreateUser).HasColumnName("Ds_CreateUser");
        }
    }
}
