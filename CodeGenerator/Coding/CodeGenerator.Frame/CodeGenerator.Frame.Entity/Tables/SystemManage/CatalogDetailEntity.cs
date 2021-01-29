using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.SystemManage
{
    /// <summary>
    /// 数据库表实体--字典明细表
    /// </summary>
    public class CatalogDetailEntity : BaseEntity
    {
        /// <summary>
        /// 明细编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 明细名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 明细值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 明细描述
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; } = 1;

        /// <summary>
        /// 所属类型ID
        /// </summary>
        public string ParentOid { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
