using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Xavier.Framework.Db.Models;

namespace Xavier.Framework.Db.Repository
{
    /// <summary>
    /// 数据库仓储接口(非泛型)
    /// </summary>
    public interface IRepositoryBase
    {
        /// <summary>
        /// 打开数据库
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭数据库
        /// </summary>
        void Close();

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns>返回结果</returns>
        int Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entity">插入实体</param>
        /// <returns>返回结果</returns>
        int Insert<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys">实体集合</param>
        /// <returns>返回结果</returns>
        int Insert<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys">实体集合</param>
        void BulkInsert<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entity"></param>
        /// <returns>返回结果</returns>
        int Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys"></param>
        /// <returns>返回结果</returns>
        int Update<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys"></param>
        void BulkUpdate<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entity"></param>
        /// <returns>返回结果</returns>
        int Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys"></param>
        /// <returns>返回结果</returns>
        int Delete<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="entitys"></param>
        void BulkDelete<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <returns>返回结果</returns>
        int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="keyValue">主键</param>
        /// <returns>返回结果</returns>
        TEntity FindEntity<TEntity>(object keyValue) where TEntity : class;

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <returns>返回结果</returns>
        TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <returns>返回结果</returns>
        IQueryable<TEntity> IQueryable<TEntity>() where TEntity : class;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <returns>返回结果</returns>
        IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <returns>返回结果</returns>
        List<TEntity> FindList<TEntity>() where TEntity : class;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="strSql">执行Sql语句</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindList<TEntity>(string strSql) where TEntity : class;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindList<TEntity>(string strSql, DbParameter[] parameters) where TEntity : class;

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="pagination">分页类</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindPage<TEntity>(Pagination pagination) where TEntity : class, new();

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <param name="pagination">分页类</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindPage<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class, new();

        /// <summary>
        /// 获取Db实体
        /// </summary>
        /// <typeparam name="TEntity">泛型实体</typeparam>
        /// <returns>返回结果</returns>
        DbSet<TEntity> Entity<TEntity>() where TEntity : class, new();

        /// <summary>
        /// 查询Scalar结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <returns>返回结果</returns>
        object FindScalar(string strSql);

        /// <summary>
        /// 查询Scalar结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        object FindScalar(string strSql, DbParameter[] parameters);

        /// <summary>
        /// 查询DataTable结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        DataTable FindDataTable(string strSql, params DbParameter[] parameters);

        /// <summary>
        /// 查询DataSet结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        DataSet FindDataSet(string strSql, params DbParameter[] parameters);

        /// <summary>
        /// 查询DataReader结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        DbDataReader FindReader(string strSql, params DbParameter[] parameters);
    }

    /// <summary>
    /// 数据库仓储接口(泛型)
    /// </summary>
    /// <typeparam name="TEntity">泛型实体</typeparam>
    public interface IRepositoryBase<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 打开数据库
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭数据库
        /// </summary>
        void Close();

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns>返回结果</returns>
        int Commit();

        /// <summary>
        /// 回滚数据
        /// </summary>
        void Rollback();

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">插入实体</param>
        /// <returns>返回结果</returns>
        int Insert(TEntity entity);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entitys">插入实体集</param>
        /// <returns>返回结果</returns>
        int Insert(List<TEntity> entitys);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entitys">插入实体集</param>
        void BulkInsert(List<TEntity> entitys);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">更新实体</param>
        /// <returns>返回结果</returns>
        int Update(TEntity entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entitys">更新实体集</param>
        /// <returns>返回结果</returns>
        int Update(List<TEntity> entitys);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entitys">更新实体集</param>
        void BulkUpdate(List<TEntity> entitys);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns>返回结果</returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entitys">删除实体集</param>
        /// <returns>返回结果</returns>
        int Delete(List<TEntity> entitys);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entitys">删除实体集</param>
        void BulkDelete(List<TEntity> entitys);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <returns>返回结果</returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns>返回结果</returns>
        TEntity FindEntity(object keyValue);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <returns>返回结果</returns>
        TEntity FindEntity(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns>返回结果</returns>
        IQueryable<TEntity> IQueryable();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <returns>返回结果</returns>
        IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns>返回结果</returns>
        List<TEntity> FindList();

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindList(string strSql);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindList(string strSql, DbParameter[] parameters);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="strSql">执行Sql语句</param>
        /// <returns>返回结果</returns>
        List<T> FindList<T>(string strSql);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        List<T> FindList<T>(string strSql, DbParameter[] parameters);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pagination">分页类</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindPage(Pagination pagination);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="predicate">执行Sql的Lamda表达式</param>
        /// <param name="pagination">分页类</param>
        /// <returns>返回结果</returns>
        List<TEntity> FindPage(Expression<Func<TEntity, bool>> predicate, Pagination pagination);

        /// <summary>
        /// 获取Db实体
        /// </summary>
        /// <typeparam name="TLinq">泛型</typeparam>
        /// <returns>返回结果</returns>
        DbSet<TLinq> Entity<TLinq>() where TLinq : class, new();

        /// <summary>
        /// 查询Scalar结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <returns>返回结果</returns>
        object FindScalar(string strSql);

        /// <summary>
        /// 查询Scalar结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        object FindScalar(string strSql, DbParameter[] parameters);

        /// <summary>
        /// 查询DataTable结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        DataTable FindDataTable(string strSql, params DbParameter[] parameters);

        /// <summary>
        /// 查询DataSet结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        DataSet FindDataSet(string strSql, params DbParameter[] parameters);

        /// <summary>
        /// 查询DataReader结果数据
        /// </summary>
        /// <param name="strSql">执行Sql语句</param>
        /// <param name="parameters">执行sql参数化</param>
        /// <returns>返回结果</returns>
        DbDataReader FindReader(string strSql, params DbParameter[] parameters);
    }
}
