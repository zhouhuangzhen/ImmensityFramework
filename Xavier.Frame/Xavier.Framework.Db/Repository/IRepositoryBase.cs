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
    /// ���ݿ�ִ��ӿ�(�Ƿ���)
    /// </summary>
    public interface IRepositoryBase
    {
        /// <summary>
        /// �����ݿ�
        /// </summary>
        void Open();

        /// <summary>
        /// �ر����ݿ�
        /// </summary>
        void Close();

        /// <summary>
        /// �ύ
        /// </summary>
        /// <returns>���ؽ��</returns>
        int Commit();

        /// <summary>
        /// �ع�
        /// </summary>
        void Rollback();

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entity">����ʵ��</param>
        /// <returns>���ؽ��</returns>
        int Insert<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys">ʵ�弯��</param>
        /// <returns>���ؽ��</returns>
        int Insert<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys">ʵ�弯��</param>
        void BulkInsert<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entity"></param>
        /// <returns>���ؽ��</returns>
        int Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys"></param>
        /// <returns>���ؽ��</returns>
        int Update<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys"></param>
        void BulkUpdate<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entity"></param>
        /// <returns>���ؽ��</returns>
        int Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys"></param>
        /// <returns>���ؽ��</returns>
        int Delete<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="entitys"></param>
        void BulkDelete<TEntity>(List<TEntity> entitys) where TEntity : class;

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <returns>���ؽ��</returns>
        int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// ��ѯʵ��
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="keyValue">����</param>
        /// <returns>���ؽ��</returns>
        TEntity FindEntity<TEntity>(object keyValue) where TEntity : class;

        /// <summary>
        /// ��ѯʵ��
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <returns>���ؽ��</returns>
        TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <returns>���ؽ��</returns>
        IQueryable<TEntity> IQueryable<TEntity>() where TEntity : class;

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <returns>���ؽ��</returns>
        IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindList<TEntity>() where TEntity : class;

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="strSql">ִ��Sql���</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindList<TEntity>(string strSql) where TEntity : class;

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindList<TEntity>(string strSql, DbParameter[] parameters) where TEntity : class;

        /// <summary>
        /// ��ѯ��ҳ
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="pagination">��ҳ��</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindPage<TEntity>(Pagination pagination) where TEntity : class, new();

        /// <summary>
        /// ��ѯ��ҳ
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <param name="pagination">��ҳ��</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindPage<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class, new();

        /// <summary>
        /// ��ȡDbʵ��
        /// </summary>
        /// <typeparam name="TEntity">����ʵ��</typeparam>
        /// <returns>���ؽ��</returns>
        DbSet<TEntity> Entity<TEntity>() where TEntity : class, new();

        /// <summary>
        /// ��ѯScalar�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <returns>���ؽ��</returns>
        object FindScalar(string strSql);

        /// <summary>
        /// ��ѯScalar�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        object FindScalar(string strSql, DbParameter[] parameters);

        /// <summary>
        /// ��ѯDataTable�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        DataTable FindDataTable(string strSql, params DbParameter[] parameters);

        /// <summary>
        /// ��ѯDataSet�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        DataSet FindDataSet(string strSql, params DbParameter[] parameters);

        /// <summary>
        /// ��ѯDataReader�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        DbDataReader FindReader(string strSql, params DbParameter[] parameters);
    }

    /// <summary>
    /// ���ݿ�ִ��ӿ�(����)
    /// </summary>
    /// <typeparam name="TEntity">����ʵ��</typeparam>
    public interface IRepositoryBase<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// �����ݿ�
        /// </summary>
        void Open();

        /// <summary>
        /// �ر����ݿ�
        /// </summary>
        void Close();

        /// <summary>
        /// �ύ����
        /// </summary>
        /// <returns>���ؽ��</returns>
        int Commit();

        /// <summary>
        /// �ع�����
        /// </summary>
        void Rollback();

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entity">����ʵ��</param>
        /// <returns>���ؽ��</returns>
        int Insert(TEntity entity);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entitys">����ʵ�弯</param>
        /// <returns>���ؽ��</returns>
        int Insert(List<TEntity> entitys);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entitys">����ʵ�弯</param>
        void BulkInsert(List<TEntity> entitys);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entity">����ʵ��</param>
        /// <returns>���ؽ��</returns>
        int Update(TEntity entity);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entitys">����ʵ�弯</param>
        /// <returns>���ؽ��</returns>
        int Update(List<TEntity> entitys);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entitys">����ʵ�弯</param>
        void BulkUpdate(List<TEntity> entitys);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="entity">ɾ��ʵ��</param>
        /// <returns>���ؽ��</returns>
        int Delete(TEntity entity);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="entitys">ɾ��ʵ�弯</param>
        /// <returns>���ؽ��</returns>
        int Delete(List<TEntity> entitys);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="entitys">ɾ��ʵ�弯</param>
        void BulkDelete(List<TEntity> entitys);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <returns>���ؽ��</returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// ��ѯʵ��
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <returns>���ؽ��</returns>
        TEntity FindEntity(object keyValue);

        /// <summary>
        /// ��ѯʵ��
        /// </summary>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <returns>���ؽ��</returns>
        TEntity FindEntity(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <returns>���ؽ��</returns>
        IQueryable<TEntity> IQueryable();

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <returns>���ؽ��</returns>
        IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindList();

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindList(string strSql);

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindList(string strSql, DbParameter[] parameters);

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="strSql">ִ��Sql���</param>
        /// <returns>���ؽ��</returns>
        List<T> FindList<T>(string strSql);

        /// <summary>
        /// ��ѯ�б�
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        List<T> FindList<T>(string strSql, DbParameter[] parameters);

        /// <summary>
        /// ��ѯ��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ��</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindPage(Pagination pagination);

        /// <summary>
        /// ��ѯ��ҳ
        /// </summary>
        /// <param name="predicate">ִ��Sql��Lamda���ʽ</param>
        /// <param name="pagination">��ҳ��</param>
        /// <returns>���ؽ��</returns>
        List<TEntity> FindPage(Expression<Func<TEntity, bool>> predicate, Pagination pagination);

        /// <summary>
        /// ��ȡDbʵ��
        /// </summary>
        /// <typeparam name="TLinq">����</typeparam>
        /// <returns>���ؽ��</returns>
        DbSet<TLinq> Entity<TLinq>() where TLinq : class, new();

        /// <summary>
        /// ��ѯScalar�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <returns>���ؽ��</returns>
        object FindScalar(string strSql);

        /// <summary>
        /// ��ѯScalar�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        object FindScalar(string strSql, DbParameter[] parameters);

        /// <summary>
        /// ��ѯDataTable�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        DataTable FindDataTable(string strSql, params DbParameter[] parameters);

        /// <summary>
        /// ��ѯDataSet�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        DataSet FindDataSet(string strSql, params DbParameter[] parameters);

        /// <summary>
        /// ��ѯDataReader�������
        /// </summary>
        /// <param name="strSql">ִ��Sql���</param>
        /// <param name="parameters">ִ��sql������</param>
        /// <returns>���ؽ��</returns>
        DbDataReader FindReader(string strSql, params DbParameter[] parameters);
    }
}
