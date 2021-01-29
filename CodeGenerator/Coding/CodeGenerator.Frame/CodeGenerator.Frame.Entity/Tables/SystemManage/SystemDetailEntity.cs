using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.SystemManage
{
    /// <summary>
    /// 数据库表实体--系统字典明细配置表
    /// </summary>
    public class SystemDetailEntity : BaseEntity
    {
        /// <summary>
        /// 字典明细Id
        /// </summary>
        public string DetailId { get; set; }

        /// <summary>
        /// 配置所属系统Id
        /// </summary>
        public string SystemId { get; set; }

        /// <summary>
        /// 系统各自配置值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
