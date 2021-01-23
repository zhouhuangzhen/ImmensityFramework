using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Xavier.Framework.Db.Commons
{
    /// <summary>
    /// context��չ��
    /// </summary>
    public static class ContextExt
    {
        #region ���ݲ�ѯ
        /// <summary>
        /// EF����DataTable��չ��
        /// </summary>
        /// <param name="database">EF���ݿ����</param>
        /// <param name="strSql">sql���</param>
        /// <returns></returns>
        public static DataTable ExcuteDataTable(this Database database, string strSql)
        {
            return ExcuteDataTable(database, strSql, new DbParameter[0]);
        }

        /// <summary>
        /// EF����DataTable��չ��
        /// </summary>
        /// <param name="database">EF���ݿ����</param>
        /// <param name="strSql">sql���</param>
        /// <param name="parameters">��������</param>
        /// <returns></returns>
        public static DataTable ExcuteDataTable(this Database database, string strSql, DbParameter[] parameters)
        {
            DbConnection conn = database.Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.ConnectionString = database.Connection.ConnectionString;
                conn.Open();
            }
            string connectionName = conn.GetType().Name;
            DbCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = strSql;
            if (parameters.Length > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
            }
            DataAdapter adapter = null;
            switch (connectionName)
            {
                case "SqlConnection":
                    {
                        adapter = new System.Data.SqlClient.SqlDataAdapter((System.Data.SqlClient.SqlCommand)cmd);
                        break;
                    }
                case "MySqlConnection":
                    {
                        adapter = new MySql.Data.MySqlClient.MySqlDataAdapter((MySql.Data.MySqlClient.MySqlCommand)cmd);
                        break;
                    }
                case "OracleConnection":
                    {
                        adapter = new Oracle.ManagedDataAccess.Client.OracleDataAdapter((Oracle.ManagedDataAccess.Client.OracleCommand)cmd);
                        break;
                    }
                case "OleDbConnection":
                    {
                        adapter = new System.Data.SQLite.SQLiteDataAdapter((System.Data.SQLite.SQLiteCommand)cmd);
                        break;
                    }
                case "NpgsqlConnection":
                    {
                        adapter = new Npgsql.NpgsqlDataAdapter((Npgsql.NpgsqlCommand)cmd);
                        break;
                    }
                default:
                    {
                        adapter = new System.Data.SqlClient.SqlDataAdapter((System.Data.SqlClient.SqlCommand)cmd);
                        break;
                    }
            }
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// EF����DataSet��չ��
        /// </summary>
        /// <param name="database">EF���ݿ����</param>
        /// <param name="strSql">sql���</param>
        /// <returns></returns>
        public static DataSet ExcuteDataSet(this Database database, string strSql)
        {
            return ExcuteDataSet(database, strSql, new DbParameter[0]);
        }

        /// <summary>
        /// EF����DataSet��չ��
        /// </summary>
        /// <param name="database">EF���ݿ����</param>
        /// <param name="strSql">sql���</param>
        /// <param name="parameters">��������</param>
        /// <returns></returns>
        public static DataSet ExcuteDataSet(this Database database, string strSql, DbParameter[] parameters)
        {
            DbConnection conn = database.Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.ConnectionString = database.Connection.ConnectionString;
                conn.Open();
            }
            string connectionName = conn.GetType().Name;
            DbCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = strSql;
            if (parameters.Length > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
            }
            DataAdapter adapter = null;
            switch (connectionName)
            {
                case "SqlConnection":
                    {
                        adapter = new System.Data.SqlClient.SqlDataAdapter((System.Data.SqlClient.SqlCommand)cmd);
                        break;
                    }
                case "MySqlConnection":
                    {
                        adapter = new MySql.Data.MySqlClient.MySqlDataAdapter((MySql.Data.MySqlClient.MySqlCommand)cmd);
                        break;
                    }
                case "OracleConnection":
                    {
                        adapter = new Oracle.ManagedDataAccess.Client.OracleDataAdapter((Oracle.ManagedDataAccess.Client.OracleCommand)cmd);
                        break;
                    }
                case "OleDbConnection":
                    {
                        adapter = new System.Data.SQLite.SQLiteDataAdapter((System.Data.SQLite.SQLiteCommand)cmd);
                        break;
                    }
                case "NpgsqlConnection":
                    {
                        adapter = new Npgsql.NpgsqlDataAdapter((Npgsql.NpgsqlCommand)cmd);
                        break;
                    }
                default:
                    {
                        adapter = new System.Data.SqlClient.SqlDataAdapter((System.Data.SqlClient.SqlCommand)cmd);
                        break;
                    }
            }
            DataSet table = new DataSet();
            adapter.Fill(table);
            return table;
        }
        #endregion

        #region ���ݿ���������

        /// <summary>
        /// ��ȡ���ݿ������
        /// </summary>
        /// <param name="database">EF���ݿ����</param>
        /// <returns></returns>
        public static string GetDbType(this Database database)
        {
            return database.Connection.GetType().Name;
        }

        /// <summary>
        /// ��ȡ�������ݿ�����
        /// </summary>
        /// <param name="database">EF���ݿ����</param>
        /// <returns></returns>
        public static string GetDatabaseName(this Database database)
        {
            return database.Connection.Database;
        }

        /// <summary>
        /// ��ȡ��������Դ
        /// </summary>
        /// <param name="database">EF���ݿ����</param>
        /// <returns></returns>
        public static string GetDataSource(this Database database)
        {
            return database.Connection.DataSource;
        }
        #endregion
    }

}
