using System;
using Xavier.Framework.Db.Models;

namespace Xavier.Framework.Db.Repository
{
    /// <summary>
    /// 数据库仓储类(非泛型)
    /// </summary>
	public class RepositoryBase : BaseContext, IRepositoryBase
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public RepositoryBase()
		{
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="schema">schema</param>
		public RepositoryBase(string schema = "dbo")
			: base(schema)
		{
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="provider">数据库类型</param>
        /// <param name="schema">schema</param>
		public RepositoryBase(string connection, DatabaseType provider, string schema = "dbo")
			: base(connection, provider, schema)
		{
		}
    }

    /// <summary>
    /// 数据库仓储类(泛型)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
	public class RepositoryBase<TEntity> : BaseContext<TEntity>, IRepositoryBase<TEntity> where TEntity : class, new()
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public RepositoryBase()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="schema">schema</param>
		public RepositoryBase(string schema = "dbo")
            : base(schema)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="provider">数据库类型</param>
        /// <param name="schema">schema</param>
		public RepositoryBase(string connection, DatabaseType provider, string schema = "dbo")
            : base(connection, provider, schema)
        {
        }
    }
}
