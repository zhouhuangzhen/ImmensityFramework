using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Entity.Commons
{
    /// <summary>
    /// 系统数据库表实体
    /// </summary>
    public class TableInfoEntity
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 表类型
        /// </summary>
        public string TableType { get; set; }
    }
}
