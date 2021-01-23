using System.ComponentModel;

namespace Xavier.Framework.Db.Models
{
    /// <summary>
    /// 数据库类型
    /// </summary>
	public enum DatabaseType
    {
        /// <summary>
        /// SqlServer
        /// </summary>
		[Description("System.Data.SqlClient")]
        SqlServer = 1,

        /// <summary>
        /// MySql
        /// </summary>
		[Description("MySql.Data.MySqlClient")]
        MySql = 2,

        /// <summary>
        /// Oracle
        /// </summary>
		[Description("Oracle.ManagedDataAccess.Client")]
        Oracle = 3,

        /// <summary>
        /// SQLite
        /// </summary>
		[Description("System.Data.SQLite.EF6")]
        SQLite = 4,

        /// <summary>
        /// Postgre
        /// </summary>
		[Description("Npgsql")]
        Postgre = 5
    }
}
