using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.CodeManage
{
    /// <summary>
    /// 数据库表实体--任务字段表
    /// </summary>
    public class TaskFieldEntity : BaseEntity
    {
        /// <summary>
        /// 系统使用表格Id
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// 表字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 字段作用(1：列表 ，2：查询 ，3：编辑 ，4：导出)
        /// </summary>
        public int Feature { get; set; }

        /// <summary>
        /// 字段排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 表字段描述
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
