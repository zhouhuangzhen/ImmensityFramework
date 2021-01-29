using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.SystemManage
{
    /// <summary>
    /// 数据库表实体--字典分类表
    /// </summary>
    public class CatalogEntity : BaseEntity
    {
        /// <summary>
        /// 分类编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典类型(1:系统参数，2:枚举参数，3:框架参数)
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 字典描述
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
