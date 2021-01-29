using CodeGenerator.Frame.Entity.Tables.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Commons;
using Xavier.Framework.Db.Models;
using Xavier.Framework.Db.Repository;

namespace CodeGenerator.Frame.Dals.Tables.SystemManage
{
    #region 数据库处理类--字典分类表
    /// <summary>
    /// 数据库处理类--字典分类表
    /// </summary>
    public class CatalogDal : IDisposable
    {
        #region 变量
        /// <summary>
        /// 数据库处理对象--字典分类表
        /// </summary>
        private readonly IRepositoryBase<CatalogEntity> baseRepository = new RepositoryBase<CatalogEntity>();
        #endregion

        #region 方法
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            baseRepository.Close();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public int Insert(CatalogEntity entity)
        {
            return baseRepository.Insert(entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        /// <returns></returns>
        public int Insert(List<CatalogEntity> entities)
        {
            return baseRepository.Insert(entities);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        public void InsertBulk(List<CatalogEntity> entities)
        {
            baseRepository.BulkInsert(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新</param>
        /// <returns></returns>
        public int Update(CatalogEntity entity)
        {
            return baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        /// <returns></returns>
        public int Update(List<CatalogEntity> entities)
        {
            return baseRepository.Update(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        public void UpdateBulk(List<CatalogEntity> entities)
        {
            baseRepository.BulkUpdate(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public int Delete(string id)
        {
            var expression = ExtLinq.True<CatalogEntity>();
            expression = expression.And(x => x.Oid.Equals(id));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">主键Ids</param>
        /// <returns></returns>
        public int Delete(string[] ids)
        {
            var expression = ExtLinq.True<CatalogEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public int Delete(CatalogEntity entity)
        {
            return baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        /// <returns></returns>
        public int Delete(List<CatalogEntity> entities)
        {
            return baseRepository.Delete(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        public void DeleteBulk(List<CatalogEntity> entities)
        {
            baseRepository.BulkDelete(entities);
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public CatalogEntity GetEntity(string id)
        {
            return baseRepository.FindEntity(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="code">分类编码</param>
        /// <param name="name">分类名称</param>
        /// <param name="type">字典类型</param>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        public List<CatalogEntity> GetPaged(string code, string name, int type, Pagination pagination)
        {
            var expression = ExtLinq.True<CatalogEntity>();
            if (!string.IsNullOrWhiteSpace(code))
            {
                expression = expression.And(x => x.Code.Contains(code));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                expression = expression.And(x => x.Name.Contains(name));
            }
            if (type != -1)
            {
                expression = expression.And(x => x.Type.Equals(type));
            }
            return baseRepository.FindPage(expression, pagination);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="code">分类编码</param>
        /// <param name="name">分类名称</param>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        public List<CatalogEntity> GetList(string code, string name, int type)
        {
            var expression = ExtLinq.True<CatalogEntity>();
            if (!string.IsNullOrWhiteSpace(code))
            {
                expression = expression.And(x => x.Code.Contains(code));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                expression = expression.And(x => x.Name.Contains(name));
            }
            if (type != -1)
            {
                expression = expression.And(x => x.Type.Equals(type));
            }
            return baseRepository.FindList(expression);
        }

        /// <summary>
        /// 根据主键Id集合获取列表数据
        /// </summary>
        /// <param name="ids">主键Id集合</param>
        /// <returns></returns>
        public List<CatalogEntity> GetList(string[] ids)
        {
            var expression = ExtLinq.True<CatalogEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.FindList(expression);
        }
        #endregion
    }
    #endregion
}
