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
    #region 数据库上下文DbContext基础处理类(无泛型)
    /// <summary>
    /// 数据库上下文DbContext基础处理类(无泛型)
    /// </summary>
    public class BaseContext
    {
        #region 参数
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private DbContext dbcontext = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseContext()
        {
            this.dbcontext = new DatabaseContext();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="schema">数据库Schema</param>
        public BaseContext(string schema = "dbo")
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            this.dbcontext = new DatabaseContext(schema);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="provider">数据库类型</param>
        /// <param name="schema">标投标识符</param>
        public BaseContext(string connection, DatabaseType provider, string schema = "dbo")
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            dbcontext = new DatabaseContext(connection, provider, schema);
        }
        #endregion

        #region 数据库处理
        /// <summary>
        /// 打开数据库
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
        /// 关闭数据库
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
        /// 事务提交
        /// </summary>
        /// <returns>返回结果</returns>
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
        /// 事务回滚
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

        #region 数据处理
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>返回结果</returns>
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
        /// 插入数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="entitys">实体对象集合</param>
        /// <returns>返回结果</returns>
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
        /// 快速插入数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys">实体集合</param>
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
        /// 更新数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>返回结果</returns>
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
        /// 更新数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="entitys">实体对象集合</param>
        /// <returns>返回结果</returns>
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
        /// 快速更新数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys">更新集合</param>
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
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>返回结果</returns>
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
        /// 根据查询表达式删除数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <returns>返回结果</returns>
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
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="entitys">实体对象集合</param>
        /// <returns>返回结果</returns>
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
        /// 快速删除数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys">删除集合</param>
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
        /// 根据key查找数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="keyValue">key</param>
        /// <returns>返回结果</returns>
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
        /// 根据查找表达式查找数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="predicate">查找表达式</param>
        /// <returns>返回结果</returns>
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
        /// 查询所有数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <returns>返回结果</returns>
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
        /// 根据查询表达式查询数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <returns>返回结果</returns>
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
        /// 根据sql语句查询数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <returns>返回结果</returns>
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
        /// 根据sql语句和参数查询数据
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns>返回结果</returns>
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
        /// 根据分页信息对所有数据进行查询分页
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="pagination">分页信息</param>
        /// <returns>返回结果</returns>
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
        /// 根据查询表达式、分页信息对相关数据进行查询分页
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <param name="predicate">查询表达式</param>
        /// <param name="pagination">分页信息</param>
        /// <returns>返回结果</returns>
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
        /// 返回实体，做linq用
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <returns></returns>
        public DbSet<TEntity> Entity<TEntity>() where TEntity : class, new()
        {
            return dbcontext.Set<TEntity>();
        }

        /// <summary>
        /// 返回Scalar
        /// </summary>
        /// <param name="strSql">查询语句</param>
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
        /// 返回Scalar
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="parameters">查询参数</param>
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
        /// 返回DataTable
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="parameters">查询参数</param>
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
        /// 返回DataTable
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="parameters">查询参数</param>
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
        /// 返回Scalar
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="parameters">查询参数</param>
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

        #region 数据库上下文配置项
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        public DatabaseType GetDbType()
        {
            DatabaseType type = default(DatabaseType);
            string dbConnection = this.dbcontext.Database.GetDbType();
            string dbType = dbConnection.Replace("Connection", "");
            switch (dbType.ToLower())
            {
                //  SqlServer数据库
                case "sql":
                    {
                        type = DatabaseType.SqlServer;
                        break;
                    }
                //  MySql数据库
                case "mysql":
                    {
                        type = DatabaseType.MySql;
                        break;
                    }
                //  SQLite数据库
                case "sqlite":
                    {
                        type = DatabaseType.SQLite;
                        break;
                    }
                //  Oracle数据库
                case "oracle":
                    {
                        type = DatabaseType.Oracle;
                        break;
                    }
                //  Npgsql(PostgreSql数据库)
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
        /// 获取连接数据库名称
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return this.dbcontext.Database.GetDatabaseName();
        }

        /// <summary>
        /// 获取连接数据源
        /// </summary>
        /// <returns></returns>
        public string GetDataSource()
        {
            return this.dbcontext.Database.GetDataSource();
        }
        #endregion
    }
    #endregion

    #region 数据库上下文DbContext基础处理类(泛型)
    /// <summary>
    /// 数据库上下文DbContext基础处理类(泛型)
    /// </summary>
    public class BaseContext<TEntity> where TEntity : class, new()
    {
        #region 参数
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private DbContext dbcontext = null;
        #endregion


        #region  构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseContext()
        {
            this.dbcontext = new DatabaseContext();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="schema">数据库Schema</param>
        public BaseContext(string schema = "dbo")
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            this.dbcontext = new DatabaseContext(schema);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="provider">数据库类型</param>
        /// <param name="schema">标投标识符</param>
        public BaseContext(string connection, DatabaseType provider, string schema = "dbo")
        {
            if (dbcontext != null)
            {
                dbcontext.Dispose();
            }
            dbcontext = new DatabaseContext(connection, provider, schema);
        }
        #endregion

        #region 数据库处理
        /// <summary>
        /// 打开数据库
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
        /// 关闭数据库
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
        /// 事务提交
        /// </summary>
        /// <returns>返回结果</returns>
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
        /// 事务回滚
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

        #region 数据处理
        /// <summary>
        /// 返回Scalar
        /// </summary>
        /// <param name="strSql">查询语句</param>
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
        /// 返回Scalar
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="parameters">查询参数</param>
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
        /// 返回DataTable
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="parameters">查询参数</param>
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
        /// 返回DataTable
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="parameters">查询参数</param>
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
        /// 返回Scalar
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="parameters">查询参数</param>
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
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns>返回信息</returns>
        public int Insert(TEntity entity)
        {
            Open();
            dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// 插入实体集
        /// </summary>
        /// <param name="entitys">实体信息集合</param>
        /// <returns>返回信息</returns>
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
        /// 快速插入实体集
        /// </summary>
        /// <param name="entitys">实体信息集合</param>
        /// <returns>返回信息</returns>
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
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns>返回信息</returns>
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
        /// 更新实体集
        /// </summary>
        /// <param name="entitys">实体信息集合</param>
        /// <returns>返回信息</returns>
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
        /// 快速更新实体集
        /// </summary>
        /// <param name="entitys">实体信息集合</param>
        /// <returns>返回信息</returns>
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
        /// 删除实体信息
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns>返回信息</returns>
        public int Delete(TEntity entity)
        {
            Open();
            dbcontext.Set<TEntity>().Attach(entity);
            dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// 删除实体集
        /// </summary>
        /// <param name="entitys">实体信息集合</param>
        /// <returns>返回信息</returns>
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
        /// 快速删除实体集
        /// </summary>
        /// <param name="entitys">实体信息集合</param>
        /// <returns>返回信息</returns>
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
        /// 根据查询表达式删除实体信息
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns>返回信息</returns>
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            Open();
            var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return this.dbcontext.Database.CurrentTransaction == null ? this.Commit() : 0;
        }

        /// <summary>
        /// 根据Key查询实体
        /// </summary>
        /// <param name="keyValue">Key</param>
        /// <returns>返回信息</returns>
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
        /// 根据查询表达式查询实体
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns>返回信息</returns>
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
        /// 获得所有的实体信息集合
        /// </summary>
        /// <returns>返回信息</returns>
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
        /// 根据查询表达式获得实体信息集合
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <returns>返回信息</returns>
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
        /// 根据sql语句获得实体信息集合
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns>返回信息</returns>
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
        /// 根据sql语句获得实体信息集合，带参数，防注入
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="dbParameter">sql参数</param>
        /// <returns>返回信息</returns>
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
        /// 根据sql语句获得实体信息集合
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns>返回信息</returns>
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
        /// 根据sql语句获得实体信息集合
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>返回信息</returns>
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
        /// 返回实体，做linq用
        /// </summary>
        /// <typeparam name="TEntity">泛型</typeparam>
        /// <returns></returns>
        public DbSet<TLinq> Entity<TLinq>() where TLinq : class, new()
        {
            return dbcontext.Set<TLinq>();
        }

        /// <summary>
        /// 根据分页信息获得所有分页的实体信息集
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <returns>返回信息</returns>
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
        /// 根据查询表达式和分页信息获得分页的实体信息集
        /// </summary>
        /// <param name="predicate">查询表达式</param>
        /// <param name="pagination">分页信息</param>
        /// <returns>返回信息</returns>
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

        #region 数据库上下文配置项
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        public DatabaseType GetDbType()
        {
            DatabaseType type = default(DatabaseType);
            string dbConnection = this.dbcontext.Database.GetDbType();
            string dbType = dbConnection.Replace("Connection", "");
            switch (dbType.ToLower())
            {
                //  SqlServer数据库
                case "sql":
                    {
                        type = DatabaseType.SqlServer;
                        break;
                    }
                //  MySql数据库
                case "mysql":
                    {
                        type = DatabaseType.MySql;
                        break;
                    }
                //  SQLite数据库
                case "sqlite":
                    {
                        type = DatabaseType.SQLite;
                        break;
                    }
                //  Oracle数据库
                case "oracle":
                    {
                        type = DatabaseType.Oracle;
                        break;
                    }
                //  Npgsql(PostgreSql数据库)
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
        /// 获取连接数据库名称
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return this.dbcontext.Database.GetDatabaseName();
        }

        /// <summary>
        /// 获取连接数据源
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
