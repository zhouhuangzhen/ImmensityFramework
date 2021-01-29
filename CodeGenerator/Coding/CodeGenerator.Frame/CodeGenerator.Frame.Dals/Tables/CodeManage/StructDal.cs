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
    #region 数据库处理类--框架结构信息表
    /// <summary>
    /// 数据库处理类--框架结构信息表
    /// </summary>
    public class StructDal : IDisposable
    {
        #region 变量
        /// <summary>
        /// 数据库处理对象--框架结构信息表
        /// </summary>
        private readonly IRepositoryBase<StructEntity> baseRepository = new RepositoryBase<StructEntity>();

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
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">新增实体</param>
        /// <returns></returns>
        public int Insert(StructEntity entity)
        {
            return baseRepository.Insert(entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        /// <returns></returns>
        public int Insert(List<StructEntity> entities)
        {
            return baseRepository.Insert(entities);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        public void InsertBulk(List<StructEntity> entities)
        {
            baseRepository.BulkInsert(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新</param>
        /// <returns></returns>
        public int Update(StructEntity entity)
        {
            return baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        /// <returns></returns>
        public int Update(List<StructEntity> entities)
        {
            return baseRepository.Update(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        public void UpdateBulk(List<StructEntity> entities)
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
            var expression = ExtLinq.True<StructEntity>();
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
            var expression = ExtLinq.True<StructEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public int Delete(StructEntity entity)
        {
            return baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        /// <returns></returns>
        public int Delete(List<StructEntity> entities)
        {
            return baseRepository.Delete(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        public void DeleteBulk(List<StructEntity> entities)
        {
            baseRepository.BulkDelete(entities);
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public StructEntity GetEntity(string id)
        {
            return baseRepository.FindEntity(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="code">结构编码</param>
        /// <param name="name">结构名称</param>
        /// <param name="frameId">系统Id</param>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        public List<StructViewModel> GetPaged(string code, string name, string frameId, Pagination pagination)
        {
            var frames = baseCommon.Entity<FrameEntity>();
            var structs = baseCommon.Entity<StructEntity>();
            var linq = from frame in frames
                       join struc in structs on frame.Oid equals struc.FrameId
                       where
                            (string.IsNullOrWhiteSpace(code) ? true : struc.Code.Contains(code)) &&
                            (string.IsNullOrWhiteSpace(name) ? true : struc.Name.Contains(name)) &&
                            (string.IsNullOrWhiteSpace(frameId) ? true : struc.FrameId.Equals(frameId))
                       orderby frame.Code ascending, struc.Order ascending
                       select new StructViewModel
                       {
                           Oid = struc.Oid,
                           Remark = struc.Remark,
                           Enable = struc.Enable,
                           InsertTime = struc.InsertTime,
                           FrameId = struc.FrameId,
                           CreateUser = struc.CreateUser,
                           StructCode = struc.Code,
                           StructName = struc.Name,
                           Order = struc.Order,
                           FrameCode = frame.Code,
                           FrameName = frame.Name
                       };
            return linq.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows).ToList();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="code">结构编码</param>
        /// <param name="name">结构名称</param>
        /// <param name="frameId">系统Id</param>
        /// <returns></returns>
        public List<StructViewModel> GetList(string code, string name, string frameId)
        {
            var frames = baseCommon.Entity<FrameEntity>();
            var structs = baseCommon.Entity<StructEntity>();
            var linq = from frame in frames
                       join struc in structs on frame.Oid equals struc.FrameId
                       where
                            (string.IsNullOrWhiteSpace(code) ? true : struc.Code.Contains(code)) &&
                            (string.IsNullOrWhiteSpace(name) ? true : struc.Name.Contains(name)) &&
                            (string.IsNullOrWhiteSpace(frameId) ? true : struc.FrameId.Equals(frameId))
                       orderby frame.Code ascending, struc.Order ascending
                       select new StructViewModel
                       {
                           Oid = struc.Oid,
                           Remark = struc.Remark,
                           Enable = struc.Enable,
                           InsertTime = struc.InsertTime,
                           FrameId = struc.FrameId,
                           CreateUser = struc.CreateUser,
                           StructCode = struc.Code,
                           StructName = struc.Name,
                           Order = struc.Order,
                           FrameCode = frame.Code,
                           FrameName = frame.Name
                       };
            return linq.ToList();
        }

        /// <summary>
        /// 根据主键Id集合获取列表数据
        /// </summary>
        /// <param name="ids">主键Id集合</param>
        /// <returns></returns>
        public List<StructEntity> GetList(string[] ids)
        {
            var expression = ExtLinq.True<StructEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.FindList(expression);
        }
        #endregion
    }
    #endregion
}
