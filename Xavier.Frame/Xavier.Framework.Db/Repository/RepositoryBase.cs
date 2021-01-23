using System;
using Xavier.Framework.Db.Models;

namespace Xavier.Framework.Db.Repository
{
    /// <summary>
    /// ���ݿ�ִ���(�Ƿ���)
    /// </summary>
	public class RepositoryBase : BaseContext, IRepositoryBase
	{
        /// <summary>
        /// ���캯��
        /// </summary>
		public RepositoryBase()
		{
		}

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="schema">schema</param>
		public RepositoryBase(string schema = "dbo")
			: base(schema)
		{
		}

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="connection">���ݿ������ַ���</param>
        /// <param name="provider">���ݿ�����</param>
        /// <param name="schema">schema</param>
		public RepositoryBase(string connection, DatabaseType provider, string schema = "dbo")
			: base(connection, provider, schema)
		{
		}
    }

    /// <summary>
    /// ���ݿ�ִ���(����)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
	public class RepositoryBase<TEntity> : BaseContext<TEntity>, IRepositoryBase<TEntity> where TEntity : class, new()
	{
        /// <summary>
        /// ���캯��
        /// </summary>
        public RepositoryBase()
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="schema">schema</param>
		public RepositoryBase(string schema = "dbo")
            : base(schema)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="connection">���ݿ������ַ���</param>
        /// <param name="provider">���ݿ�����</param>
        /// <param name="schema">schema</param>
		public RepositoryBase(string connection, DatabaseType provider, string schema = "dbo")
            : base(connection, provider, schema)
        {
        }
    }
}
