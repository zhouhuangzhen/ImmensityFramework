using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.CodeManage
{
    /// <summary>
    /// 数据库表实体--框架模块表
    /// </summary>
    public class ModuleEntity : BaseEntity<ModuleEntity>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属结构Id
        /// </summary>
        public string StructId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Dll名称
        /// </summary>
        public string Dll { get; set; }

        /// <summary>
        /// Dll执行方法
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// 保存Dll的文件Id
        /// </summary>
        public string DllId { get; set; }

        /// <summary>
        /// Dll版本号
        /// </summary>
        public string DllVersion { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
