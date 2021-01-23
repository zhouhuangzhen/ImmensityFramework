using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Entity.ViewModels
{
    /// <summary>
    /// 视图模型--任务数据表
    /// </summary>
    public class TableViewModel
    {
        /// <summary>
        /// 数据唯一标识
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int Enable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 系统Id
        /// </summary>
        public string SystemId { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 数据库类型(DatabaseType : 1:Oracle , 2:MySQL , 3:Oracle , 4:SQLite , 5:Postgre)
        /// </summary>
        public int SystemType { get; set; }

        /// <summary>
        /// 解决方案名称
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 数据库表Schema
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 系统表
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 系统表名称
        /// </summary>
        public string TableComment { get; set; }

    }
}
