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
    /// 数据库表映射--任务数据表
    /// </summary>
    public class TaskTableMapping : EntityTypeConfiguration<TaskTableEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskTableMapping()
        {
            this.ToTable("Code_TaskTable");
            this.HasKey(x => x.Oid);
            this.Property(x => x.SysId).HasColumnName("Task_SysId");
            this.Property(x => x.TableName).HasColumnName("Task_TableName");
            this.Property(x => x.TableComment).HasColumnName("Task_TableComment");
            this.Property(x => x.CreateUser).HasColumnName("Table_CreateUser");
        }
    }
}
