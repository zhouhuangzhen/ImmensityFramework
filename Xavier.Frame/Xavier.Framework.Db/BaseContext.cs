using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Xavier.Framework.Db.Commons;
using Xavier.Framework.Db.Models;

namespace Xavier.Framework.Db
{
    #region ���ݿ�������DbContext����������(�޷���)
    /// <summary>
    /// ���ݿ�������DbContext����������(�޷���)
    /// </summary>
    public class BaseContext
    {
        #region ����
        /// <summary>
        /// ���ݿ�������
        /// </summary>
        private DbContext dbcontext = null;
        #endregion

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public BaseContext()
        {
            this.dbcontext = new DatabaseContext();
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="schema">���ݿ�Schema</param>
        public BaseContext(string schema = "dbo")
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            this.dbcontext = new DatabaseContext(schema);
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="connection">���ݿ������ַ���</param>
        /// <param name="provider">���ݿ�����</param>
        /// <param name="schema">��Ͷ��ʶ��</param>
        public BaseContext(string connection, DatabaseType provider, string schema = "dbo")
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            dbcontext = new DatabaseContext(connection, provider, schema);
        }
        #endregion

        #region ���ݿ⴦��
        /// <summary>
        /// �����ݿ�
        /// </summary>
        public void Open()
        {
            DbConnection dbConnection = ((IObjectContextAdapter)dbcontext).ObjectContext.Connection;
            if (dbConnection != null && dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
        }

        /// <summary>
        /// �ر����ݿ�
        /// </summary>
        public void Close()
        {
            DbConnection dbConnection = ((IObjectContextAdapter)dbcontext).ObjectContext.Connection;
            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }

        /// <summary>
        /// �����ύ
        /// </summary>
        /// <returns>���ؽ��</returns>
        public int Commit()
        {
            try
            {
                var returnValue = dbcontext.SaveChanges();
                if (this.dbcontext.Database.CurrentTransaction != null)
                {
                    this.dbcontext.Database.CurrentTransaction.Commit();
                }
                return returnValue;
            }
            catch (Exception)
            {
                if (this.dbcontext.Database.CurrentTransaction != null)
                {
                    this.dbcontext.Database.CurrentTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����ع�
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (this.dbcontext.Database.CurrentTransaction != null)
                {
                    this.dbcontext.Database.CurrentTransaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Close();
            }
        }

        #endregion

        #region ���ݴ���
        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="entity">ʵ�����</param>
        /// <returns>���ؽ��</returns>
        public int Insert<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                this.Open();
                dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
                return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="entitys">ʵ����󼯺�</param>
        /// <returns>���ؽ��</returns>
        public int Insert<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            try
            {
                this.Open();
                foreach (var entity in entitys)
                {
                    dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
                }
                return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ���ٲ�������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys">ʵ�弯��</param>
        /// <returns></returns>
        public void BulkInsert<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            try
            {
                this.Open();
                foreach (var entity in entitys)
                {
                    dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
                }
                dbcontext.BulkInsert<TEntity>(entitys);
                //  return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
                this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="entity">ʵ�����</param>
        /// <returns>���ؽ��</returns>
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                this.Open();
                dbcontext.Set<TEntity>().Attach(entity);
                PropertyInfo[] props = entity.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.GetValue(entity, null) != null)
                    {
                        if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                            dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                        dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                }
                return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="entitys">ʵ����󼯺�</param>
        /// <returns>���ؽ��</returns>
        public int Update<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            try
            {
                this.Open();
                foreach (var entity in entitys)
                {
                    dbcontext.Set<TEntity>().Attach(entity);
                    PropertyInfo[] props = entity.GetType().GetProperties();
                    foreach (PropertyInfo prop in props)
                    {
                        if (prop.GetValue(entity, null) != null)
                        {
                            if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                                dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                            dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                        }
                    }
                }
                return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ���ٸ�������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys">���¼���</param>
        public void BulkUpdate<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            try
            {
                this.Open();
                foreach (var entity in entitys)
                {
                    dbcontext.Set<TEntity>().Attach(entity);
                    PropertyInfo[] props = entity.GetType().GetProperties();
                    foreach (PropertyInfo prop in props)
                    {
                        if (prop.GetValue(entity, null) != null)
                        {
                            if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                                dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                            dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                        }
                    }
                }
                dbcontext.BulkUpdate<TEntity>(entitys);
                //  return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="entity">ʵ�����</param>
        /// <returns>���ؽ��</returns>
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                this.Open();
                dbcontext.Set<TEntity>().Attach(entity);
                dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
                return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ���ݲ�ѯ���ʽɾ������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="predicate">��ѯ���ʽ</param>
        /// <returns>���ؽ��</returns>
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            try
            {
                this.Open();
                var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
                entitys.ForEach(m => dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
                return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="entitys">ʵ����󼯺�</param>
        /// <returns>���ؽ��</returns>
        public int Delete<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            try
            {
                this.Open();
                foreach (var entity in entitys)
                {
                    dbcontext.Set<TEntity>().Attach(entity);
                    dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
                }
                return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����ɾ������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys">ɾ������</param>
        public void BulkDelete<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            try
            {
                this.Open();
                foreach (var entity in entitys)
                {
                    dbcontext.Set<TEntity>().Attach(entity);
                    PropertyInfo[] props = entity.GetType().GetProperties();
                    foreach (PropertyInfo prop in props)
                    {
                        dbcontext.Set<TEntity>().Attach(entity);
                        dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
                    }
                }
                dbcontext.BulkDelete<TEntity>(entitys);
                //  return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����key��������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="keyValue">key</param>
        /// <returns>���ؽ��</returns>
        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class
        {
            try
            {
                this.Open();
                return dbcontext.Set<TEntity>().Find(keyValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                this.Close();
            }
        }

        /// <summary>
        /// ���ݲ��ұ��ʽ��������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="predicate">���ұ��ʽ</param>
        /// <returns>���ؽ��</returns>
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            try
            {
                Open();
                return dbcontext.Set<TEntity>().FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                this.Close();
            }
        }

        /// <summary>
        /// ��ѯ��������
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <returns>���ؽ��</returns>
        public IQueryable<TEntity> IQueryable<TEntity>() where TEntity : class
        {
            try
            {
                this.Open();
                return dbcontext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                this.Close();
            }
        }

        /// <summary>
        /// ���ݲ�ѯ���ʽ��ѯ����
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="predicate">��ѯ���ʽ</param>
        /// <returns>���ؽ��</returns>
        public IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            try
            {
                this.Open();
                return dbcontext.Set<TEntity>().Where(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                this.Close();
            }
        }

        /// <summary>
        /// ����sql����ѯ����
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="strSql">sql���</param>
        /// <returns>���ؽ��</returns>
        public List<TEntity> FindList<TEntity>(string strSql) where TEntity : class
        {
            try
            {
                this.Open();
                return dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                this.Close();
            }
        }

        /// <summary>
        /// ����sql���Ͳ�����ѯ����
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="strSql">sql���</param>
        /// <param name="dbParameter">����</param>
        /// <returns>���ؽ��</returns>
        public List<TEntity> FindList<TEntity>(string strSql, DbParameter[] dbParameter) where TEntity : class
        {
            try
            {
                this.Open();
                return dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                this.Close();
            }
        }

        /// <summary>
        /// ���ݷ�ҳ��Ϣ���������ݽ��в�ѯ��ҳ
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="pagination">��ҳ��Ϣ</param>
        /// <returns>���ؽ��</returns>
        public List<TEntity> FindPage<TEntity>(Pagination pagination) where TEntity : class, new()
        {
            try
            {
                Open();
                bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
                string[] _order = pagination.sidx.Split(',');
                MethodCallExpression resultExp = null;
                var tempData = dbcontext.Set<TEntity>().AsQueryable();
                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool sort = isAsc;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(TEntity), "t");
                    var property = typeof(TEntity).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
                tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
                pagination.records = tempData.Count();
                tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
                return tempData.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

                this.Close();
            }
        }

        /// <summary>
        /// ���ݲ�ѯ���ʽ����ҳ��Ϣ��������ݽ��в�ѯ��ҳ
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <param name="predicate">��ѯ���ʽ</param>
        /// <param name="pagination">��ҳ��Ϣ</param>
        /// <returns>���ؽ��</returns>
        public List<TEntity> FindPage<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class, new()
        {
            try
            {
                Open();
                bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
                string[] _order = pagination.sidx.Split(',');
                MethodCallExpression resultExp = null;
                var tempData = dbcontext.Set<TEntity>().Where(predicate);
                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool sort = isAsc;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(TEntity), "t");
                    var property = typeof(TEntity).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
                tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
                pagination.records = tempData.Count();
                tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
                return tempData.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����ʵ�壬��linq��
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <returns></returns>
        public DbSet<TEntity> Entity<TEntity>() where TEntity : class, new()
        {
            return dbcontext.Set<TEntity>();
        }

        /// <summary>
        /// ����Scalar
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <returns></returns>
        public object FindScalar(string strSql)
        {
            try
            {
                this.Open();
                using (var cmd = this.dbcontext.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = strSql;
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����Scalar
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns></returns>
        public object FindScalar(string strSql, DbParameter[] parameters)
        {
            try
            {
                Open();
                using (var cmd = this.dbcontext.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = strSql;
                    cmd.Parameters.AddRange(parameters);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����DataTable
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns></returns>
        public DataTable FindDataTable(string strSql, params DbParameter[] parameters)
        {
            DataTable table = new DataTable();
            try
            {
                Open();
                table = this.dbcontext.Database.ExcuteDataTable(strSql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
            return table;
        }

        /// <summary>
        /// ����DataTable
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns></returns>
        public DataSet FindDataSet(string strSql, params DbParameter[] parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                Open();
                ds = this.dbcontext.Database.ExcuteDataSet(strSql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
            return ds;
        }

        /// <summary>
        /// ����Scalar
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns></returns>
        public DbDataReader FindReader(string strSql, params DbParameter[] parameters)
        {
            DbDataReader reader = null;
            try
            {
                Open();
                using (var cmd = this.dbcontext.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = strSql;
                    cmd.Parameters.AddRange(parameters);
                    reader = cmd.ExecuteReader();
                    cmd.Parameters.Clear();
                    return reader;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
            return reader;
        }
        #endregion

        #region ���ݿ�������������
        /// <summary>
        /// ��ȡ���ݿ�����
        /// </summary>
        /// <returns></returns>
        public DatabaseType GetDbType()
        {
            DatabaseType type = default(DatabaseType);
            string dbConnection = this.dbcontext.Database.GetDbType();
            string dbType = dbConnection.Replace("Connection", "");
            switch (dbType.ToLower())
            {
                //  SqlServer���ݿ�
                case "sql":
                    {
                        type = DatabaseType.SqlServer;
                        break;
                    }
                //  MySql���ݿ�
                case "mysql":
                    {
                        type = DatabaseType.MySql;
                        break;
                    }
                //  SQLite���ݿ�
                case "sqlite":
                    {
                        type = DatabaseType.SQLite;
                        break;
                    }
                //  Oracle���ݿ�
                case "oracle":
                    {
                        type = DatabaseType.Oracle;
                        break;
                    }
                //  Npgsql(PostgreSql���ݿ�)
                case "npgsql":
                    {
                        type = DatabaseType.Postgre;
                        break;
                    }
                default:
                    {
                        type = DatabaseType.SqlServer;
                        break;
                    }
            }
            return type;
        }

        /// <summary>
        /// ��ȡ�������ݿ�����
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return this.dbcontext.Database.GetDatabaseName();
        }

        /// <summary>
        /// ��ȡ��������Դ
        /// </summary>
        /// <returns></returns>
        public string GetDataSource()
        {
            return this.dbcontext.Database.GetDataSource();
        }
        #endregion
    }
    #endregion

    #region ���ݿ�������DbContext����������(����)
    /// <summary>
    /// ���ݿ�������DbContext����������(����)
    /// </summary>
    public class BaseContext<TEntity> where TEntity : class, new()
    {
        #region ����
        /// <summary>
        /// ���ݿ�������
        /// </summary>
        private DbContext dbcontext = null;
        #endregion


        #region  ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public BaseContext()
        {
            this.dbcontext = new DatabaseContext();
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="schema">���ݿ�Schema</param>
        public BaseContext(string schema = "dbo")
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            this.dbcontext = new DatabaseContext(schema);
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="connection">���ݿ������ַ���</param>
        /// <param name="provider">���ݿ�����</param>
        /// <param name="schema">��Ͷ��ʶ��</param>
        public BaseContext(string connection, DatabaseType provider, string schema = "dbo")
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            dbcontext = new DatabaseContext(connection, provider, schema);
        }
        #endregion

        #region ���ݿ⴦��
        /// <summary>
        /// �����ݿ�
        /// </summary>
        public void Open()
        {
            DbConnection dbConnection = ((IObjectContextAdapter)dbcontext).ObjectContext.Connection;
            if (dbConnection != null && dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
        }

        /// <summary>
        /// �ر����ݿ�
        /// </summary>
        public void Close()
        {
            DbConnection dbConnection = ((IObjectContextAdapter)dbcontext).ObjectContext.Connection;
            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();
            }
        }

        /// <summary>
        /// �����ύ
        /// </summary>
        /// <returns>���ؽ��</returns>
        public int Commit()
        {
            try
            {
                Open();
                var returnValue = dbcontext.SaveChanges();
                if (this.dbcontext.Database.CurrentTransaction != null)
                {
                    this.dbcontext.Database.CurrentTransaction.Commit();
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                if (this.dbcontext.Database.CurrentTransaction != null)
                {
                    this.dbcontext.Database.CurrentTransaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (this.dbcontext.Database.CurrentTransaction != null)
                {
                    this.dbcontext.Database.CurrentTransaction.Dispose();
                }
                this.Close();
            }
        }

        /// <summary>
        /// ����ع�
        /// </summary>
        public void Rollback()
        {
            try
            {
                Open();
                if (this.dbcontext.Database.CurrentTransaction != null)
                {
                    this.dbcontext.Database.CurrentTransaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.dbcontext.Database.CurrentTransaction != null)
                {
                    this.dbcontext.Database.CurrentTransaction.Dispose();
                }
                this.Close();
            }
        }
        #endregion

        #region ���ݴ���
        /// <summary>
        /// ����Scalar
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <returns></returns>
        public object FindScalar(string strSql)
        {
            try
            {
                this.Open();
                using (var cmd = this.dbcontext.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = strSql;
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����Scalar
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns></returns>
        public object FindScalar(string strSql, DbParameter[] parameters)
        {
            try
            {
                Open();
                using (var cmd = this.dbcontext.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = strSql;
                    cmd.Parameters.AddRange(parameters);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����DataTable
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns></returns>
        public DataTable FindDataTable(string strSql, params DbParameter[] parameters)
        {
            DataTable table = new DataTable();
            try
            {
                Open();
                table = this.dbcontext.Database.ExcuteDataTable(strSql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
            return table;
        }

        /// <summary>
        /// ����DataTable
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns></returns>
        public DataSet FindDataSet(string strSql, params DbParameter[] parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                Open();
                ds = this.dbcontext.Database.ExcuteDataSet(strSql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
            return ds;
        }

        /// <summary>
        /// ����Scalar
        /// </summary>
        /// <param name="strSql">��ѯ���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns></returns>
        public DbDataReader FindReader(string strSql, params DbParameter[] parameters)
        {
            DbDataReader reader = null;
            try
            {
                Open();
                using (var cmd = this.dbcontext.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = strSql;
                    cmd.Parameters.AddRange(parameters);
                    reader = cmd.ExecuteReader();
                    cmd.Parameters.Clear();
                    return reader;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
            return reader;
        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="entity">ʵ����Ϣ</param>
        /// <returns>������Ϣ</returns>
        public int Insert(TEntity entity)
        {
            Open();
            dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ����ʵ�弯
        /// </summary>
        /// <param name="entitys">ʵ����Ϣ����</param>
        /// <returns>������Ϣ</returns>
        public int Insert(List<TEntity> entitys)
        {
            Open();
            foreach (var entity in entitys)
            {
                dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ���ٲ���ʵ�弯
        /// </summary>
        /// <param name="entitys">ʵ����Ϣ����</param>
        /// <returns>������Ϣ</returns>
        public void BulkInsert(List<TEntity> entitys)
        {
            Open();
            foreach (var entity in entitys)
            {
                dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            dbcontext.BulkInsert<TEntity>(entitys);
            Close();
            //  return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="entity">ʵ����Ϣ</param>
        /// <returns>������Ϣ</returns>
        public int Update(TEntity entity)
        {
            Open();
            dbcontext.Set<TEntity>().Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ����ʵ�弯
        /// </summary>
        /// <param name="entitys">ʵ����Ϣ����</param>
        /// <returns>������Ϣ</returns>
        public int Update(List<TEntity> entitys)
        {
            Open();
            foreach (var entity in entitys)
            {
                dbcontext.Set<TEntity>().Attach(entity);
                //dbcontext.Entry<TEntity>(entity).State = EntityState.Detached;
                //dbcontext.Entry<TEntity>(entity).State = EntityState.Modified;
                PropertyInfo[] props = entity.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.GetValue(entity, null) != null)
                    {
                        if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                            dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                        dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                }
            }
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ���ٸ���ʵ�弯
        /// </summary>
        /// <param name="entitys">ʵ����Ϣ����</param>
        /// <returns>������Ϣ</returns>
        public void BulkUpdate(List<TEntity> entitys)
        {
            Open();
            foreach (var entity in entitys)
            {
                dbcontext.Set<TEntity>().Attach(entity);
                //dbcontext.Entry<TEntity>(entity).State = EntityState.Detached;
                //dbcontext.Entry<TEntity>(entity).State = EntityState.Modified;
                PropertyInfo[] props = entity.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.GetValue(entity, null) != null)
                    {
                        if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                            dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                        dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                }
            }
            dbcontext.BulkUpdate<TEntity>(entitys);
            Close();
            //return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ɾ��ʵ����Ϣ
        /// </summary>
        /// <param name="entity">ʵ����Ϣ</param>
        /// <returns>������Ϣ</returns>
        public int Delete(TEntity entity)
        {
            Open();
            dbcontext.Set<TEntity>().Attach(entity);
            dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ɾ��ʵ�弯
        /// </summary>
        /// <param name="entitys">ʵ����Ϣ����</param>
        /// <returns>������Ϣ</returns>
        public int Delete(List<TEntity> entitys)
        {
            Open();
            foreach (var entity in entitys)
            {
                dbcontext.Set<TEntity>().Attach(entity);
                dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            }
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ����ɾ��ʵ�弯
        /// </summary>
        /// <param name="entitys">ʵ����Ϣ����</param>
        /// <returns>������Ϣ</returns>
        public void BulkDelete(List<TEntity> entitys)
        {
            Open();
            foreach (var entity in entitys)
            {
                dbcontext.Set<TEntity>().Attach(entity);
                dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            }
            dbcontext.BulkDelete<TEntity>(entitys);
            Close();
            //  return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ���ݲ�ѯ���ʽɾ��ʵ����Ϣ
        /// </summary>
        /// <param name="predicate">��ѯ���ʽ</param>
        /// <returns>������Ϣ</returns>
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            Open();
            var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// ����Key��ѯʵ��
        /// </summary>
        /// <param name="keyValue">Key</param>
        /// <returns>������Ϣ</returns>
        public TEntity FindEntity(object keyValue)
        {
            try
            {
                Open();
                return dbcontext.Set<TEntity>().Find(keyValue);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ���ݲ�ѯ���ʽ��ѯʵ��
        /// </summary>
        /// <param name="predicate">��ѯ���ʽ</param>
        /// <returns>������Ϣ</returns>
        public TEntity FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                Open();
                return dbcontext.Set<TEntity>().FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ������е�ʵ����Ϣ����
        /// </summary>
        /// <returns>������Ϣ</returns>
        public IQueryable<TEntity> IQueryable()
        {
            try
            {
                Open();
                return dbcontext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ���ݲ�ѯ���ʽ���ʵ����Ϣ����
        /// </summary>
        /// <param name="predicate">��ѯ���ʽ</param>
        /// <returns>������Ϣ</returns>
        public IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                Open();
                return dbcontext.Set<TEntity>().Where(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����sql�����ʵ����Ϣ����
        /// </summary>
        /// <param name="strSql">sql���</param>
        /// <returns>������Ϣ</returns>
        public List<TEntity> FindList(string strSql)
        {
            try
            {
                Open();
                return dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����sql�����ʵ����Ϣ���ϣ�����������ע��
        /// </summary>
        /// <param name="strSql">sql���</param>
        /// <param name="dbParameter">sql����</param>
        /// <returns>������Ϣ</returns>
        public List<TEntity> FindList(string strSql, DbParameter[] dbParameter)
        {
            try
            {
                Open();
                return dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����sql�����ʵ����Ϣ����
        /// </summary>
        /// <param name="strSql">sql���</param>
        /// <returns>������Ϣ</returns>
        public List<T> FindList<T>(string strSql)
        {
            try
            {
                Open();
                return dbcontext.Database.SqlQuery<T>(strSql).ToList<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����sql�����ʵ����Ϣ����
        /// </summary>
        /// <param name="strSql">sql���</param>
        /// <param name="parameters">��ѯ����</param>
        /// <returns>������Ϣ</returns>
        public List<T> FindList<T>(string strSql, DbParameter[] parameters)
        {
            try
            {
                Open();
                return dbcontext.Database.SqlQuery<T>(strSql, parameters).ToList<T>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ����ʵ�壬��linq��
        /// </summary>
        /// <typeparam name="TEntity">����</typeparam>
        /// <returns></returns>
        public DbSet<TLinq> Entity<TLinq>() where TLinq : class, new()
        {
            return dbcontext.Set<TLinq>();
        }

        /// <summary>
        /// ���ݷ�ҳ��Ϣ������з�ҳ��ʵ����Ϣ��
        /// </summary>
        /// <param name="pagination">��ҳ��Ϣ</param>
        /// <returns>������Ϣ</returns>
        public List<TEntity> FindPage(Pagination pagination)
        {
            try
            {
                Open();
                bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
                string[] _order = pagination.sidx.Split(',');
                MethodCallExpression resultExp = null;
                var tempData = dbcontext.Set<TEntity>().AsQueryable();
                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool sort = isAsc;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(TEntity), "t");
                    var property = typeof(TEntity).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
                tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
                pagination.records = tempData.Count();
                tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
                return tempData.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }

        /// <summary>
        /// ���ݲ�ѯ���ʽ�ͷ�ҳ��Ϣ��÷�ҳ��ʵ����Ϣ��
        /// </summary>
        /// <param name="predicate">��ѯ���ʽ</param>
        /// <param name="pagination">��ҳ��Ϣ</param>
        /// <returns>������Ϣ</returns>
        public List<TEntity> FindPage(Expression<Func<TEntity, bool>> predicate, Pagination pagination)
        {
            try
            {
                Open();
                bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
                string[] _order = pagination.sidx.Split(',');
                MethodCallExpression resultExp = null;
                var tempData = dbcontext.Set<TEntity>().Where(predicate);
                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool sort = isAsc;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(TEntity), "t");
                    var property = typeof(TEntity).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
                tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
                pagination.records = tempData.Count();
                tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
                return tempData.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }
        #endregion

        #region ���ݿ�������������
        /// <summary>
        /// ��ȡ���ݿ�����
        /// </summary>
        /// <returns></returns>
        public DatabaseType GetDbType()
        {
            DatabaseType type = default(DatabaseType);
            string dbConnection = this.dbcontext.Database.GetDbType();
            string dbType = dbConnection.Replace("Connection", "");
            switch (dbType.ToLower())
            {
                //  SqlServer���ݿ�
                case "sql":
                    {
                        type = DatabaseType.SqlServer;
                        break;
                    }
                //  MySql���ݿ�
                case "mysql":
                    {
                        type = DatabaseType.MySql;
                        break;
                    }
                //  SQLite���ݿ�
                case "sqlite":
                    {
                        type = DatabaseType.SQLite;
                        break;
                    }
                //  Oracle���ݿ�
                case "oracle":
                    {
                        type = DatabaseType.Oracle;
                        break;
                    }
                //  Npgsql(PostgreSql���ݿ�)
                case "npgsql":
                    {
                        type = DatabaseType.Postgre;
                        break;
                    }
                default:
                    {
                        type = DatabaseType.SqlServer;
                        break;
                    }
            }
            return type;
        }

        /// <summary>
        /// ��ȡ�������ݿ�����
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return this.dbcontext.Database.GetDatabaseName();
        }

        /// <summary>
        /// ��ȡ��������Դ
        /// </summary>
        /// <returns></returns>
        public string GetDataSource()
        {
            return this.dbcontext.Database.GetDataSource();
        }
        #endregion

    }
    #endregion
}
