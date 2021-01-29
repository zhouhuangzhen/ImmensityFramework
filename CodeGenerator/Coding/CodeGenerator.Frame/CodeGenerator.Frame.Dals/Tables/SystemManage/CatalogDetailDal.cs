using CodeGenerator.Frame.Entity.Tables.SystemManage;
using CodeGenerator.Frame.Entity.Views;
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
    #region 数据库处理类--字典明细表
    /// <summary>
    /// 数据库处理类--字典明细表
    /// </summary>
    public class CatalogDetailDal : IDisposable
    {
        #region 变量
        /// <summary>
        /// 数据库处理对象--字典明细表
        /// </summary>
        private readonly IRepositoryBase<CatalogDetailEntity> baseRepository = new RepositoryBase<CatalogDetailEntity>();

        /// <summary>
        /// 数据库处理对象--字典视图
        /// </summary>
        private readonly IRepositoryBase<ViewCatalogEntity> repoViewCatalog = new RepositoryBase<ViewCatalogEntity>();
        #endregion

        #region 方法
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            baseRepository.Close();
            this.repoViewCatalog.Close();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public int Insert(CatalogDetailEntity entity)
        {
            return baseRepository.Insert(entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        /// <returns></returns>
        public int Insert(List<CatalogDetailEntity> entities)
        {
            return baseRepository.Insert(entities);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        public void InsertBulk(List<CatalogDetailEntity> entities)
        {
            baseRepository.BulkInsert(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新</param>
        /// <returns></returns>
        public int Update(CatalogDetailEntity entity)
        {
            return baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        /// <returns></returns>
        public int Update(List<CatalogDetailEntity> entities)
        {
            return baseRepository.Update(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        public void UpdateBulk(List<CatalogDetailEntity> entities)
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
            var expression = ExtLinq.True<CatalogDetailEntity>();
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
            var expression = ExtLinq.True<CatalogDetailEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public int Delete(CatalogDetailEntity entity)
        {
            return baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        /// <returns></returns>
        public int Delete(List<CatalogDetailEntity> entities)
        {
            return baseRepository.Delete(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        public void DeleteBulk(List<CatalogDetailEntity> entities)
        {
            baseRepository.BulkDelete(entities);
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public CatalogDetailEntity GetEntity(string id)
        {
            return baseRepository.FindEntity(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="code">分类编码</param>
        /// <param name="name">分类名称</param>
        /// <param name="type">字典类型</param>
        /// <param name="parentId">所属类型Id</param>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        public List<ViewCatalogEntity> GetPaged(string code, string name, int type, string parentId, Pagination pagination)
        {
            var expression = ExtLinq.True<ViewCatalogEntity>();
            if (!string.IsNullOrWhiteSpace(code))
            {
                expression = expression.And(x => x.ItemCode.Contains(code));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                expression = expression.And(x => x.ItemName.Contains(name));
            }
            if (type != -1)
            {
                expression = expression.And(x => x.CatalogType.Equals(type));
            }
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                expression = expression.And(x => x.ItemParentOid.Equals(parentId));
            }
            return repoViewCatalog.FindPage(expression, pagination);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="code">分类编码</param>
        /// <param name="name">分类名称</param>
        /// <param name="type">字典类型</param>
        /// <param name="parentId">所属类型Id</param>
        /// <returns></returns>
        public List<ViewCatalogEntity> GetList(string code, string name, int type, string parentId)
        {
            var expression = ExtLinq.True<ViewCatalogEntity>();
            if (!string.IsNullOrWhiteSpace(code))
            {
                expression = expression.And(x => x.ItemCode.Contains(code));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                expression = expression.And(x => x.ItemName.Contains(name));
            }
            if (type != -1)
            {
                expression = expression.And(x => x.CatalogType.Equals(type));
            }
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                expression = expression.And(x => x.ItemParentOid.Equals(parentId));
            }
            return repoViewCatalog.FindList(expression);
        }

        /// <summary>
        /// 根据主键Id集合获取列表数据
        /// </summary>
        /// <param name="ids">主键Id集合</param>
        /// <returns></returns>
        public List<CatalogDetailEntity> GetList(string[] ids)
        {
            var expression = ExtLinq.True<CatalogDetailEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.FindList(expression);
        }
        #endregion
    }
    #endregion
}
