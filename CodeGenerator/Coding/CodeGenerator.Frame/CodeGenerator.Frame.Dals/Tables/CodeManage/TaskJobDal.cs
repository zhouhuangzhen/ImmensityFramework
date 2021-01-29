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
    #region 数据库处理类--任务生成表
    /// <summary>
    /// 数据库处理类--任务生成表
    /// </summary>
    public class TaskJobDal
    {
        #region 变量
        /// <summary>
        /// 数据库处理对象--字典分类表
        /// </summary>
        private readonly IRepositoryBase<TaskJobEntity> baseRepository = new RepositoryBase<TaskJobEntity>();

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
        public int Insert(TaskJobEntity entity)
        {
            return baseRepository.Insert(entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        /// <returns></returns>
        public int Insert(List<TaskJobEntity> entities)
        {
            return baseRepository.Insert(entities);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        public void InsertBulk(List<TaskJobEntity> entities)
        {
            baseRepository.BulkInsert(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新</param>
        /// <returns></returns>
        public int Update(TaskJobEntity entity)
        {
            return baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        /// <returns></returns>
        public int Update(List<TaskJobEntity> entities)
        {
            return baseRepository.Update(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        public void UpdateBulk(List<TaskJobEntity> entities)
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
            var expression = ExtLinq.True<TaskJobEntity>();
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
            var expression = ExtLinq.True<TaskJobEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public int Delete(TaskJobEntity entity)
        {
            return baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        /// <returns></returns>
        public int Delete(List<TaskJobEntity> entities)
        {
            return baseRepository.Delete(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        public void DeleteBulk(List<TaskJobEntity> entities)
        {
            baseRepository.BulkDelete(entities);
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public TaskJobEntity GetEntity(string id)
        {
            return baseRepository.FindEntity(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="frameId">框架Id</param>
        /// <param name="structId">结构Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="systemId">系统ID</param>
        /// <param name="type">数据库类型</param>
        /// <param name="ip">服务器IP</param>
        /// <param name="solution">解决方案名称</param>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        public List<TaskJobViewModel> GetPaged(string frameId, string structId, string moduleId, string systemId, int type, string ip, string solution, Pagination pagination)
        {
            var jobs = baseCommon.Entity<TaskJobEntity>();
            var modules = baseCommon.Entity<ModuleEntity>();
            var structs = baseCommon.Entity<StructEntity>();
            var frames = baseCommon.Entity<FrameEntity>();
            var systems = baseCommon.Entity<SystemEntity>();
            var linq = from job in jobs
                       join system in systems on job.SysId equals system.Oid
                       join module in modules on job.ModuleId equals module.Oid
                       join struc in structs on module.StructId equals struc.Oid
                       join frame in frames on struc.FrameId equals frame.Oid
                       where
                            (string.IsNullOrWhiteSpace(frameId) ? true : frame.Oid.Equals(frameId)) &&
                            (string.IsNullOrWhiteSpace(structId) ? true : struc.Oid.Equals(structId)) &&
                            (string.IsNullOrWhiteSpace(moduleId) ? true : module.Oid.Equals(moduleId)) &&
                            (string.IsNullOrWhiteSpace(systemId) ? true : system.Oid.Equals(systemId)) &&
                            (type == -1 ? true : system.Type.Equals(type)) &&
                            (string.IsNullOrWhiteSpace(ip) ? true : system.IP.Equals(ip)) &&
                            (string.IsNullOrWhiteSpace(solution) ? true : system.Solution.Equals(solution))
                       orderby system.Code ascending, frame.Code ascending, struc.Order ascending, module.Order ascending
                       select new TaskJobViewModel
                       {
                           Oid = job.Oid,
                           Enable = job.Enable,
                           Remark = job.Remark,
                           InsertTime = job.InsertTime,
                           CreateUser = job.CreateUser,
                           SystemId = system.Oid,
                           SystemCode = system.Code,
                           SystemName = system.Name,
                           SystemType = system.Type,
                           IP = system.IP,
                           Config = system.Config,
                           Solution = system.Solution,
                           Schema = system.Schema,
                           FrameId = frame.Oid,
                           FrameCode = frame.Code,
                           FrameName = frame.Name,
                           StructId = struc.Oid,
                           StructCode = struc.Code,
                           StructName = struc.Name,
                           ModuleId = module.Oid,
                           ModuleCode = module.Code,
                           ModuleName = module.Name,
                           Dll = module.Dll,
                           DllId = module.DllId,
                           DllVersion = module.DllVersion,
                           Function = module.Function,
                           Order = module.Order
                       };
            return linq.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows).ToList();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="frameId">框架Id</param>
        /// <param name="structId">结构Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="systemId">系统ID</param>
        /// <param name="type">数据库类型</param>
        /// <param name="ip">服务器IP</param>
        /// <param name="solution">解决方案名称</param>
        /// <returns></returns>
        public List<TaskJobViewModel> GetList(string frameId, string structId, string moduleId, string systemId, int type, string ip, string solution)
        {
            var jobs = baseCommon.Entity<TaskJobEntity>();
            var modules = baseCommon.Entity<ModuleEntity>();
            var structs = baseCommon.Entity<StructEntity>();
            var frames = baseCommon.Entity<FrameEntity>();
            var systems = baseCommon.Entity<SystemEntity>();
            var linq = from job in jobs
                       join system in systems on job.SysId equals system.Oid
                       join module in modules on job.ModuleId equals module.Oid
                       join struc in structs on module.StructId equals struc.Oid
                       join frame in frames on struc.FrameId equals frame.Oid
                       where
                            (string.IsNullOrWhiteSpace(frameId) ? true : frame.Oid.Equals(frameId)) &&
                            (string.IsNullOrWhiteSpace(structId) ? true : struc.Oid.Equals(structId)) &&
                            (string.IsNullOrWhiteSpace(moduleId) ? true : module.Oid.Equals(moduleId)) &&
                            (string.IsNullOrWhiteSpace(systemId) ? true : system.Oid.Equals(systemId)) &&
                            (type == -1 ? true : system.Type.Equals(type)) &&
                            (string.IsNullOrWhiteSpace(ip) ? true : system.IP.Equals(ip)) &&
                            (string.IsNullOrWhiteSpace(solution) ? true : system.Solution.Equals(solution))
                       orderby system.Code ascending, frame.Code ascending, struc.Order ascending, module.Order ascending
                       select new TaskJobViewModel
                       {
                           Oid = job.Oid,
                           Enable = job.Enable,
                           Remark = job.Remark,
                           InsertTime = job.InsertTime,
                           CreateUser = job.CreateUser,
                           SystemId = system.Oid,
                           SystemCode = system.Code,
                           SystemName = system.Name,
                           SystemType = system.Type,
                           IP = system.IP,
                           Config = system.Config,
                           Solution = system.Solution,
                           Schema = system.Schema,
                           FrameId = frame.Oid,
                           FrameCode = frame.Code,
                           FrameName = frame.Name,
                           StructId = struc.Oid,
                           StructCode = struc.Code,
                           StructName = struc.Name,
                           ModuleId = module.Oid,
                           ModuleCode = module.Code,
                           ModuleName = module.Name,
                           Dll = module.Dll,
                           DllId = module.DllId,
                           DllVersion = module.DllVersion,
                           Function = module.Function,
                           Order = module.Order
                       };
            return linq.ToList();
        }

        /// <summary>
        /// 根据主键Id集合获取列表数据
        /// </summary>
        /// <param name="ids">主键Id集合</param>
        /// <returns></returns>
        public List<TaskJobEntity> GetList(string[] ids)
        {
            var expression = ExtLinq.True<TaskJobEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.FindList(expression);
        }
        #endregion
    }
    #endregion
}
