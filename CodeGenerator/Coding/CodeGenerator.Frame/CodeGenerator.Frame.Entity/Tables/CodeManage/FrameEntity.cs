using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.CodeManage
{
    /// <summary>
    /// 数据库表实体--框架表
    /// </summary>
    public class FrameEntity : BaseEntity
    {
        /// <summary>
        /// 框架编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 框架名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
