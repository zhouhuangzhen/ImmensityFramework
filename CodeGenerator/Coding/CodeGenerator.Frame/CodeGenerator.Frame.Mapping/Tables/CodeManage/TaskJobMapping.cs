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
    /// 数据库表映射--任务生成表
    /// </summary>
    public class TaskJobMapping : EntityTypeConfiguration<TaskJobEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskJobMapping()
        {
            this.ToTable("Code_TaskJob");
            this.HasKey(x => x.Oid);
            this.Property(x => x.SysId).HasColumnName("Job_SysId");
            this.Property(x => x.ModuleId).HasColumnName("Job_ModuleId");
            this.Property(x => x.CreateUser).HasColumnName("Job_CreateUser");
        }
    }
}
