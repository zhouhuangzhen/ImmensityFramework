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
    /// 数据库表映射--任务字段表
    /// </summary>
    public class TaskFieldMapping : EntityTypeConfiguration<TaskFieldEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskFieldMapping()
        {
            this.ToTable("Code_TaskField");
            this.HasKey(x => x.Oid);
            this.Property(x => x.TableId).HasColumnName("Task_TableId");
            this.Property(x => x.Field).HasColumnName("Task_Field");
            this.Property(x => x.Feature).HasColumnName("Task_Feature");
            this.Property(x => x.Order).HasColumnName("Task_Order");
            this.Property(x => x.Comment).HasColumnName("Task_Comment");
            this.Property(x => x.CreateUser).HasColumnName("Field_CreateUser");
        }
    }
}
