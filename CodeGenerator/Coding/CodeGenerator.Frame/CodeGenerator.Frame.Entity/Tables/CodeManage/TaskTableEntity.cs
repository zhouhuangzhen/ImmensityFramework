using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.CodeManage
{
    /// <summary>
    /// 数据库表实体--任务表格表
    /// </summary>
    public class TaskTableEntity : BaseEntity
    {
        /// <summary>
        /// 系统Id
        /// </summary>
        public string SysId { get; set; }

        /// <summary>
        /// 系统表
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 系统表名称
        /// </summary>
        public string TableComment { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
