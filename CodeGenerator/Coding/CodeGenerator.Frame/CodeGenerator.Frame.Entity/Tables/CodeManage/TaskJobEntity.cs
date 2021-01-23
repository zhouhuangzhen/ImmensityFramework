using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.CodeManage
{
    /// <summary>
    /// 数据库表实体--任务生成表
    /// </summary>
    public class TaskJobEntity : BaseEntity<TaskJobEntity>
    {
        /// <summary>
        /// 任务所属系统Id
        /// </summary>
        public string SysId { get; set; }

        /// <summary>
        /// 任务所属模块Id
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
