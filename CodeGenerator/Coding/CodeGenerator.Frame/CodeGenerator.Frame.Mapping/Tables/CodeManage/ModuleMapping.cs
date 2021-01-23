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
    /// 数据库表映射--结构模块表
    /// </summary>
    class ModuleMapping : EntityTypeConfiguration<ModuleEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ModuleMapping()
        {
            this.ToTable("Code_Module");
            this.HasKey(x => x.Oid);
            this.Property(x => x.Code).HasColumnName("Module_Code");
            this.Property(x => x.Name).HasColumnName("Module_Name");
            this.Property(x => x.StructId).HasColumnName("Module_StructId");
            this.Property(x => x.Order).HasColumnName("Module_Order");
            this.Property(x => x.Dll).HasColumnName("Module_Dll");
            this.Property(x => x.Function).HasColumnName("Module_Function");
            this.Property(x => x.DllId).HasColumnName("Module_Dll_Id");
            this.Property(x => x.DllVersion).HasColumnName("Module_Dll_Version");
            this.Property(x => x.CreateUser).HasColumnName("Module_CreateUser");
        }
    }
}
