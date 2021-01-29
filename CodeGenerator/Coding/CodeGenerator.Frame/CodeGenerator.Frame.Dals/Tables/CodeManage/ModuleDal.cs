using CodeGenerator.Frame.Entity.Tables.CodeManage;
using CodeGenerator.Frame.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Commons;
using Xavier.Framework.Db.Models;
using Xavier.Framework.Db.Repository;

namespace CodeGenerator.Frame.Dals.Tables.CodeManage
{
    #region 数据库处理类--框架模块信息表
    /// <summary>
    /// 数据库处理类--框架模块信息表
    /// </summary>
    public class ModuleDal
    {
        #region 变量
        /// <summary>
        /// 数据库处理对象--字典分类表
        /// </summary>
        private readonly IRepositoryBase<ModuleEntity> baseRepository = new RepositoryBase<ModuleEntity>();

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
        public int Insert(ModuleEntity entity)
        {
            return baseRepository.Insert(entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        /// <returns></returns>
        public int Insert(List<ModuleEntity> entities)
        {
            return baseRepository.Insert(entities);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        public void InsertBulk(List<ModuleEntity> entities)
        {
            baseRepository.BulkInsert(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新</param>
        /// <returns></returns>
        public int Update(ModuleEntity entity)
        {
            return baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        /// <returns></returns>
        public int Update(List<ModuleEntity> entities)
        {
            return baseRepository.Update(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        public void UpdateBulk(List<ModuleEntity> entities)
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
            var expression = ExtLinq.True<ModuleEntity>();
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
            var expression = ExtLinq.True<ModuleEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public int Delete(ModuleEntity entity)
        {
            return baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        /// <returns></returns>
        public int Delete(List<ModuleEntity> entities)
        {
            return baseRepository.Delete(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        public void DeleteBulk(List<ModuleEntity> entities)
        {
            baseRepository.BulkDelete(entities);
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public ModuleEntity GetEntity(string id)
        {
            return baseRepository.FindEntity(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="frameId"></param>
        /// <param name="structId"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="dllName"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<ModuleViewModel> GetPaged(string frameId, string structId, string code, string name, string dllName, Pagination pagination)
        {
            var frames = baseCommon.Entity<FrameEntity>();
            var structs = baseCommon.Entity<StructEntity>();
            var modules = baseCommon.Entity<ModuleEntity>();
            var linq = from module in modules
                       join struc in structs on module.StructId equals struc.Oid
                       join frame in frames on struc.FrameId equals frame.Oid
                       where
                            (string.IsNullOrWhiteSpace(frameId) ? true : frame.Oid.Equals(frameId)) &&
                            (string.IsNullOrWhiteSpace(structId) ? true : struc.Oid.Equals(structId)) &&
                            (string.IsNullOrWhiteSpace(code) ? true : module.Code.Contains(code)) &&
                            (string.IsNullOrWhiteSpace(name) ? true : module.Name.Contains(name)) &&
                            (string.IsNullOrWhiteSpace(dllName) ? true : module.Dll.Contains(dllName))
                       orderby frame.Code ascending, struc.Code ascending, module.Order ascending
                       select new ModuleViewModel
                       {
                           Oid = module.Oid,
                           Enable = module.Enable,
                           InsertTime = module.InsertTime,
                           Remark = module.Remark,
                           CreateUser = module.CreateUser,
                           Dll = module.Dll,
                           DllId = module.DllId,
                           DllVersion = module.DllVersion,
                           Function = module.Function,
                           Order = module.Order,
                           ModuleCode = module.Code,
                           ModuleName = module.Name,
                           FrameId = frame.Oid,
                           FrameCode = frame.Code,
                           FrameName = frame.Name,
                           StructId = struc.Oid,
                           StructCode = struc.Code,
                           StructName = struc.Name
                       };
            return linq.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows).ToList();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="code">分类编码</param>
        /// <param name="name">分类名称</param>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        public List<ModuleViewModel> GetList(string frameId, string structId, string code, string name, string dllName )
        {
            var frames = baseCommon.Entity<FrameEntity>();
            var structs = baseCommon.Entity<StructEntity>();
            var modules = baseCommon.Entity<ModuleEntity>();
            var linq = from module in modules
                       join struc in structs on module.StructId equals struc.Oid
                       join frame in frames on struc.FrameId equals frame.Oid
                       where
                            (string.IsNullOrWhiteSpace(frameId) ? true : frame.Oid.Equals(frameId)) &&
                            (string.IsNullOrWhiteSpace(structId) ? true : struc.Oid.Equals(structId)) &&
                            (string.IsNullOrWhiteSpace(code) ? true : module.Code.Contains(code)) &&
                            (string.IsNullOrWhiteSpace(name) ? true : module.Name.Contains(name)) &&
                            (string.IsNullOrWhiteSpace(dllName) ? true : module.Dll.Contains(dllName))
                       orderby frame.Code ascending, struc.Code ascending, module.Order ascending
                       select new ModuleViewModel
                       {
                           Oid = module.Oid,
                           Enable = module.Enable,
                           InsertTime = module.InsertTime,
                           Remark = module.Remark,
                           CreateUser = module.CreateUser,
                           Dll = module.Dll,
                           DllId = module.DllId,
                           DllVersion = module.DllVersion,
                           Function = module.Function,
                           Order = module.Order,
                           ModuleCode = module.Code,
                           ModuleName = module.Name,
                           FrameId = frame.Oid,
                           FrameCode = frame.Code,
                           FrameName = frame.Name,
                           StructId = struc.Oid,
                           StructCode = struc.Code,
                           StructName = struc.Name
                       };
            return linq.ToList();
        }

        /// <summary>
        /// 根据主键Id集合获取列表数据
        /// </summary>
        /// <param name="ids">主键Id集合</param>
        /// <returns></returns>
        public List<ModuleEntity> GetList(string[] ids)
        {
            var expression = ExtLinq.True<ModuleEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.FindList(expression);
        }
        #endregion
    }
    #endregion
}
