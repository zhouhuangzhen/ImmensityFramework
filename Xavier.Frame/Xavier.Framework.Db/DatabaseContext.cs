using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using Xavier.Framework.Common.Helpers;
using Xavier.Framework.Db.DbConfigurations;
using Xavier.Framework.Db.Models;

namespace Xavier.Framework.Db
{
    /// <summary>
    /// 数据库上下文对象
    /// 类初始化的执行顺序
    /// 1: 子类静态变量
    /// 2: 子类静态构造函数
    /// 3: 子类非静态变量
    /// 4: 父类静态变量
    /// 5: 父类静态构造函数
    /// 6: 父类非静态变量
    /// 7: 父类构造函数
    /// 8: 子类构造函数
    /// </summary>
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    [DbConfigurationType(typeof(DatabaseConfiguration))]
    public class DatabaseContext : DbContext
    {
        private string schema = "dbo";

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DatabaseContext()
        {
            //DbConfiguration.SetConfiguration()
            //  设置不检查Miguration的历史版本变更记录表
            Database.SetInitializer<DatabaseContext>(null);
            //  DbConfiguration.SetConfiguration(new DatabaseConfiguration());
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="provider">数据库类型</param>
        /// <returns></returns>
        public static DbConnection GetConnection(string connection, DatabaseType provider)
        {

            DataTable table = DbProviderFactories.GetFactoryClasses();
            string providerName = EnumHelper.Item2Description<DatabaseType>(provider);
            DbConnection conn = null;
            // Display each row and column value.
            foreach (DataRow row in table.Rows)
            {
                if (providerName.Equals(row["InvariantName"].ToString()))
                {
                    conn = DbProviderFactories.GetFactory(row).CreateConnection();
                    break;
                }
            }
            if (conn != null)
            {
                conn.ConnectionString = connection;
            }
            return conn;
        }

        /// <summary>
        /// 初始化数据库配置项
        /// </summary>
        public void InitConfiguration()
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="provider">数据库类型</param>
        /// <param name="schema">数据库头表示</param>
        public DatabaseContext(string connection, DatabaseType provider, string schema = "dbo") : base(GetConnection(connection, provider), true)
        {
            this.schema = schema;
            InitConfiguration();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DatabaseContext(string schema = "dbo") : base("DbConnection")
        {
            this.schema = schema;
            InitConfiguration();
        }

        /// <summary>
        /// 实体创建时的重载方法
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string dllMapping = ConfigurationManager.AppSettings["MappingDll"] != null ? ConfigurationManager.AppSettings["MappingDll"].ToString() : string.Empty;
            string assembleFileName = Assembly.GetExecutingAssembly().CodeBase.Replace("Xavier.Framework.Db.DLL", dllMapping + ".DLL").Replace("file:///", "");
            Assembly asm = Assembly.LoadFile(assembleFileName);
            var typesToRegister = asm.GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            modelBuilder.HasDefaultSchema(schema);
            base.OnModelCreating(modelBuilder);
        }
    }

}
