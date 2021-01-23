using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Tables.CodeManage
{
    /// <summary>
    /// 数据库表实体--系统菜单表
    /// </summary>
    public class SystemEntity : BaseEntity<SystemEntity>
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
        public int Type { get; set; }

        /// <summary>
        /// 解决方案名称
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 数据库表Schema
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
    }
}
