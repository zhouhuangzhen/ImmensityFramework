using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Reflection;

namespace Xavier.Framework.Db.DbConfigurations
{
	public class DatabaseConfiguration : DbConfiguration
	{
		public DatabaseConfiguration()
		{
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["DbConnection"];
			if (connectionStringSettings != null && !string.IsNullOrEmpty(connectionStringSettings.ConnectionString))
			{
				switch (connectionStringSettings.ProviderName)
				{
				case "MySql.Data.MySqlClient":
					this.SetProviderFactory("MySql.Data.MySqlClient", (DbProviderFactory)MySqlClientFactory.Instance);
					break;
				case "Oracle.ManagedDataAccess.Client":
					this.SetProviderFactory("Oracle.ManagedDataAccess.Client", (DbProviderFactory)(object)OracleClientFactory.Instance);
					break;
				case "System.Data.SQLite.EF6":
				{
					this.SetProviderFactory("System.Data.SQLite", (DbProviderFactory)(object)SQLiteFactory.Instance);
					this.SetProviderFactory("System.Data.SQLite.EF6", (DbProviderFactory)(object)SQLiteProviderFactory.Instance);
					Type type = Type.GetType("System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
					FieldInfo field = type.GetField("Instance", BindingFlags.Static | BindingFlags.NonPublic);
					this.SetProviderServices("System.Data.SQLite", (DbProviderServices)(object)(DbProviderServices)field.GetValue(null));
					break;
				}
				case "System.Data.SqlClient":
					this.SetProviderFactory("System.Data.SqlClient", (DbProviderFactory)SqlClientFactory.Instance);
					break;
				default:
					this.SetProviderFactory("System.Data.SqlClient", (DbProviderFactory)SqlClientFactory.Instance);
					break;
				}
			}
		}
	}
}
