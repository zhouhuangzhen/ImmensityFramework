using CodeGenerator.Frame.Entity.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;
using Xavier.Framework.Db.Repository;

namespace CodeGenerator.Frame.Dals.Commons
{
    #region 数据处理类--基础处理
    /// <summary>
    /// 数据处理类--基础处理
    /// </summary>
    public class BaseDal : RepositoryBase, IDisposable
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseDal() : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="schema">表所属Schema</param>
        public BaseDal(string schema = "dbo") : base(schema)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="schema">表所属Schema</param>
        public BaseDal(string connection, DatabaseType dbType, string schema = "dbo") : base(connection, dbType, schema)
        {

        }
        #endregion

        #region 方法
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            base.Close();
        }

        /// <summary>
        /// 获取数据库中的表信息
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="schema">表所属Schema</param>
        /// <returns></returns>
        public List<TableInfoEntity> GetTables(string connection, DatabaseType dbType, string schema = "dbo")
        {
            return null;
        }

        /// <summary>
        /// 获取数据库表中的字段信息
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="schema">表所属Schema</param>
        /// <returns></returns>
        public List<FieldInfoEntity> GetFields(string connection, DatabaseType dbType, string tableName = "", string schema = "dbo")
        {
            return null;
        }
        #endregion
    }
    #endregion
}
