using CodeGenerator.Frame.Entity.Tables.CodeManage;
using CodeGenerator.Frame.Entity.Tables.SystemManage;
using CodeGenerator.Frame.Entity.ViewModels;
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
    #region 数据库处理类--企业配置明细表
    /// <summary>
    /// 数据库处理类--企业配置明细表
    /// </summary>
    public class SystemDetailDal : IDisposable
    {
        #region 变量
        /// <summary>
        /// 数据库处理对象--字典分类表
        /// </summary>
        private readonly IRepositoryBase<SystemDetailEntity> baseRepository = new RepositoryBase<SystemDetailEntity>();

        /// <summary>
        /// 数据库处理对象--通用
        /// </summary>
        private readonly IRepositoryBase baseCommon = new RepositoryBase();
        #endregion

        #region 方法
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            baseRepository.Close();
            baseCommon.Close();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public int Insert(SystemDetailEntity entity)
        {
            return baseRepository.Insert(entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        /// <returns></returns>
        public int Insert(List<SystemDetailEntity> entities)
        {
            return baseRepository.Insert(entities);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        public void InsertBulk(List<SystemDetailEntity> entities)
        {
            baseRepository.BulkInsert(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新</param>
        /// <returns></returns>
        public int Update(SystemDetailEntity entity)
        {
            return baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        /// <returns></returns>
        public int Update(List<SystemDetailEntity> entities)
        {
            return baseRepository.Update(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        public void UpdateBulk(List<SystemDetailEntity> entities)
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
            var expression = ExtLinq.True<SystemDetailEntity>();
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
            var expression = ExtLinq.True<SystemDetailEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public int Delete(SystemDetailEntity entity)
        {
            return baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        /// <returns></returns>
        public int Delete(List<SystemDetailEntity> entities)
        {
            return baseRepository.Delete(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        public void DeleteBulk(List<SystemDetailEntity> entities)
        {
            baseRepository.BulkDelete(entities);
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public SystemDetailEntity GetEntity(string id)
        {
            return baseRepository.FindEntity(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="typeId">字典类型Id</param>
        /// <param name="itemId">字典明细Id</param>
        /// <param name="systemId">系统信息Id</param>
        /// <param name="systemType">系统数据库类型</param>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        public List<SystemDetailViewModel> GetPaged(string typeId, string itemId, string systemId, int systemType, Pagination pagination)
        {
            var details = baseCommon.Entity<SystemDetailEntity>();
            var types = baseCommon.Entity<CatalogEntity>();
            var items = baseCommon.Entity<CatalogDetailEntity>();
            var systems = baseCommon.Entity<SystemEntity>();
            var linq = from detail in details
                       join item in items on detail.DetailId equals item.Oid
                       join type in types on item.ParentOid equals type.Oid
                       join system in systems on detail.SystemId equals system.Oid
                       where
                            (string.IsNullOrWhiteSpace(typeId) ? true : type.Oid.Equals(typeId)) &&
                            (string.IsNullOrWhiteSpace(itemId) ? true : item.Oid.Equals(itemId)) &&
                            (string.IsNullOrWhiteSpace(systemId) ? true : system.Oid.Equals(systemId)) &&
                            (systemType == -1 ? true : system.Type.Equals(type))
                       orderby system.Code ascending, item.Order ascending
                       select new SystemDetailViewModel
                       {
                           Oid = detail.Oid,
                           Remark = detail.Remark,
                           Enable = detail.Enable,
                           InsertTime = detail.InsertTime,
                           CreateUser = detail.CreateUser,
                           Value = detail.Value,
                           ItemParentOid = type.Oid,
                           CatalogCode = type.Code,
                           CatalogName = type.Name,
                           CatalogType = type.Type,
                           CatalogText = type.Text,
                           ItemId = item.Oid,
                           ItemCode = item.Code,
                           ItemName = item.Name,
                           ItemText = item.Text,
                           ItemOrder = item.Order,
                           ItemValue = item.Value,
                           SystemId = system.Oid,
                           SystemCode = system.Code,
                           SystemName = system.Name,
                           Solution = system.Solution,
                           Config = system.Config,
                           IP = system.IP,
                           Schema = system.Schema,
                           SystemType = system.Type
                       };
            return linq.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows).ToList();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="typeId">字典类型Id</param>
        /// <param name="itemId">字典明细Id</param>
        /// <param name="systemId">系统信息Id</param>
        /// <param name="systemType">系统数据库类型</param>
        /// <returns></returns>
        public List<SystemDetailViewModel> GetList(string typeId, string itemId, string systemId, int systemType)
        {
            var details = baseCommon.Entity<SystemDetailEntity>();
            var types = baseCommon.Entity<CatalogEntity>();
            var items = baseCommon.Entity<CatalogDetailEntity>();
            var systems = baseCommon.Entity<SystemEntity>();
            var linq = from detail in details
                       join item in items on detail.DetailId equals item.Oid
                       join type in types on item.ParentOid equals type.Oid
                       join system in systems on detail.SystemId equals system.Oid
                       where
                            (string.IsNullOrWhiteSpace(typeId) ? true : type.Oid.Equals(typeId)) &&
                            (string.IsNullOrWhiteSpace(itemId) ? true : item.Oid.Equals(itemId)) &&
                            (string.IsNullOrWhiteSpace(systemId) ? true : system.Oid.Equals(systemId)) &&
                            (systemType == -1 ? true : system.Type.Equals(type))
                       orderby system.Code ascending, item.Order ascending
                       select new SystemDetailViewModel
                       {
                           Oid = detail.Oid,
                           Remark = detail.Remark,
                           Enable = detail.Enable,
                           InsertTime = detail.InsertTime,
                           CreateUser = detail.CreateUser,
                           Value = detail.Value,
                           ItemParentOid = type.Oid,
                           CatalogCode = type.Code,
                           CatalogName = type.Name,
                           CatalogType = type.Type,
                           CatalogText = type.Text,
                           ItemId = item.Oid,
                           ItemCode = item.Code,
                           ItemName = item.Name,
                           ItemText = item.Text,
                           ItemOrder = item.Order,
                           ItemValue = item.Value,
                           SystemId = system.Oid,
                           SystemCode = system.Code,
                           SystemName = system.Name,
                           Solution = system.Solution,
                           Config = system.Config,
                           IP = system.IP,
                           Schema = system.Schema,
                           SystemType = system.Type
                       };
            return linq.ToList();
        }

        /// <summary>
        /// 根据主键Id集合获取列表数据
        /// </summary>
        /// <param name="ids">主键Id集合</param>
        /// <returns></returns>
        public List<SystemDetailEntity> GetList(string[] ids)
        {
            var expression = ExtLinq.True<SystemDetailEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.FindList(expression);
        }
        #endregion
    }
    #endregion
}
