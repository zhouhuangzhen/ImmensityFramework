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
    /// ���ݿ������Ķ���
    /// ���ʼ����ִ��˳��
    /// 1: ���ྲ̬����
    /// 2: ���ྲ̬���캯��
    /// 3: ����Ǿ�̬����
    /// 4: ���ྲ̬����
    /// 5: ���ྲ̬���캯��
    /// 6: ����Ǿ�̬����
    /// 7: ���๹�캯��
    /// 8: ���๹�캯��
    /// </summary>
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    [DbConfigurationType(typeof(DatabaseConfiguration))]
    public class DatabaseContext : DbContext
    {
        private string schema = "dbo";

        /// <summary>
        /// ��̬���캯��
        /// </summary>
        static DatabaseContext()
        {
            //DbConfiguration.SetConfiguration()
            //  ���ò����Miguration����ʷ�汾�����¼��
            Database.SetInitializer<DatabaseContext>(null);
            //  DbConfiguration.SetConfiguration(new DatabaseConfiguration());
        }

        /// <summary>
        /// ��ȡ���ݿ�����
        /// </summary>
        /// <param name="connection">���ݿ������ַ���</param>
        /// <param name="provider">���ݿ�����</param>
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
        /// ��ʼ�����ݿ�������
        /// </summary>
        public void InitConfiguration()
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="connection">���ݿ������ַ���</param>
        /// <param name="provider">���ݿ�����</param>
        /// <param name="schema">���ݿ�ͷ��ʾ</param>
        public DatabaseContext(string connection, DatabaseType provider, string schema = "dbo") : base(GetConnection(connection, provider), true)
        {
            this.schema = schema;
            InitConfiguration();
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public DatabaseContext(string schema = "dbo") : base("DbConnection")
        {
            this.schema = schema;
            InitConfiguration();
        }

        /// <summary>
        /// ʵ�崴��ʱ�����ط���
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
