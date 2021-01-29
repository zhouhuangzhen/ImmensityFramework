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
    #region 数据库处理类--任务字段表
    /// <summary>
    /// 数据库处理类--任务字段表
    /// </summary>
    public class TaskFieldDal
    {
        #region 变量
        /// <summary>
        /// 数据库处理对象--字典分类表
        /// </summary>
        private readonly IRepositoryBase<TaskFieldEntity> baseRepository = new RepositoryBase<TaskFieldEntity>();

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
        public int Insert(TaskFieldEntity entity)
        {
            return baseRepository.Insert(entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        /// <returns></returns>
        public int Insert(List<TaskFieldEntity> entities)
        {
            return baseRepository.Insert(entities);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        public void InsertBulk(List<TaskFieldEntity> entities)
        {
            baseRepository.BulkInsert(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新</param>
        /// <returns></returns>
        public int Update(TaskFieldEntity entity)
        {
            return baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        /// <returns></returns>
        public int Update(List<TaskFieldEntity> entities)
        {
            return baseRepository.Update(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        public void UpdateBulk(List<TaskFieldEntity> entities)
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
            var expression = ExtLinq.True<TaskFieldEntity>();
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
            var expression = ExtLinq.True<TaskFieldEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public int Delete(TaskFieldEntity entity)
        {
            return baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        /// <returns></returns>
        public int Delete(List<TaskFieldEntity> entities)
        {
            return baseRepository.Delete(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        public void DeleteBulk(List<TaskFieldEntity> entities)
        {
            baseRepository.BulkDelete(entities);
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public TaskFieldEntity GetEntity(string id)
        {
            return baseRepository.FindEntity(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="code">系统编码</param>
        /// <param name="name">系统名称</param>
        /// <param name="systemId">系统Id</param>
        /// <param name="type">数据库类型</param>
        /// <param name="ip">服务器IP</param>
        /// <param name="solution">解决方案名称</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableComment">表备注</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldComment">字段备注</param>
        /// <param name="feature">字段用途</param>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        public List<FieldViewModel> GetPaged(string code, string name, string systemId, int type, string ip, string solution, string tableName, string tableComment, string fieldName, string fieldComment, int feature, Pagination pagination)
        {
            var fields = baseCommon.Entity<TaskFieldEntity>();
            var tables = baseCommon.Entity<TaskTableEntity>();
            var systems = baseCommon.Entity<SystemEntity>();
            var linq = from field in fields
                       join table in tables on field.TableId equals table.Oid
                       join system in systems on table.SysId equals system.Oid
                       where
                            (string.IsNullOrWhiteSpace(code) ? true : system.Code.Contains(code)) &&
                            (string.IsNullOrWhiteSpace(name) ? true : system.Name.Contains(name)) &&
                            (string.IsNullOrWhiteSpace(systemId) ? true : system.Oid.Equals(systemId)) &&
                            (type == -1 ? true : system.Type.Equals(type)) &&
                            (string.IsNullOrWhiteSpace(ip) ? true : system.IP.Contains(ip)) &&
                            (string.IsNullOrWhiteSpace(solution) ? true : system.Solution.Contains(solution)) &&
                            (string.IsNullOrWhiteSpace(tableName) ? true : table.TableName.Contains(tableName)) &&
                            (string.IsNullOrWhiteSpace(tableComment) ? true : table.TableComment.Contains(tableComment)) &&
                            (string.IsNullOrWhiteSpace(fieldName) ? true : field.Field.Contains(fieldName)) &&
                            (string.IsNullOrWhiteSpace(fieldComment) ? true : field.Comment.Equals(fieldComment)) &&
                            (feature == -1 ? true : field.Feature.Equals(feature))
                       orderby system.Code ascending, table.TableName ascending, field.Order
                       select new FieldViewModel
                       {
                           Oid = field.Oid,
                           InsertTime = field.InsertTime,
                           Remark = field.Remark,
                           Enable = field.Enable,
                           Order = field.Order,
                           Field = field.Field,
                           Feature = field.Feature,
                           Comment = field.Comment,
                           CreateUser = field.CreateUser,
                           SystemId = system.Oid,
                           SystemCode = system.Code,
                           SystemName = system.Name,
                           SystemType = system.Type,
                           Config = system.Config,
                           IP = system.IP,
                           Schema = system.Schema,
                           Solution = system.Solution,
                           TableId = table.Oid,
                           TableName = table.TableName,
                           TableComment = table.TableComment
                       };
            return linq.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows).ToList();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="code">系统编码</param>
        /// <param name="name">系统名称</param>
        /// <param name="systemId">系统Id</param>
        /// <param name="type">数据库类型</param>
        /// <param name="ip">服务器IP</param>
        /// <param name="solution">解决方案名称</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableComment">表备注</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldComment">字段备注</param>
        /// <param name="feature">字段用途</param>
        /// <returns></returns>
        public List<FieldViewModel> GetList(string code, string name, string systemId, int type, string ip, string solution, string tableName, string tableComment, string fieldName, string fieldComment, int feature)
        {
            var fields = baseCommon.Entity<TaskFieldEntity>();
            var tables = baseCommon.Entity<TaskTableEntity>();
            var systems = baseCommon.Entity<SystemEntity>();
            var linq = from field in fields
                       join table in tables on field.TableId equals table.Oid
                       join system in systems on table.SysId equals system.Oid
                       where
                            (string.IsNullOrWhiteSpace(code) ? true : system.Code.Contains(code)) &&
                            (string.IsNullOrWhiteSpace(name) ? true : system.Name.Contains(name)) &&
                            (string.IsNullOrWhiteSpace(systemId) ? true : system.Oid.Equals(systemId)) &&
                            (type == -1 ? true : system.Type.Equals(type)) &&
                            (string.IsNullOrWhiteSpace(ip) ? true : system.IP.Contains(ip)) &&
                            (string.IsNullOrWhiteSpace(solution) ? true : system.Solution.Contains(solution)) &&
                            (string.IsNullOrWhiteSpace(tableName) ? true : table.TableName.Contains(tableName)) &&
                            (string.IsNullOrWhiteSpace(tableComment) ? true : table.TableComment.Contains(tableComment)) &&
                            (string.IsNullOrWhiteSpace(fieldName) ? true : field.Field.Contains(fieldName)) &&
                            (string.IsNullOrWhiteSpace(fieldComment) ? true : field.Comment.Equals(fieldComment)) &&
                            (feature == -1 ? true : field.Feature.Equals(feature))
                       orderby system.Code ascending, table.TableName ascending, field.Order
                       select new FieldViewModel
                       {
                           Oid = field.Oid,
                           InsertTime = field.InsertTime,
                           Remark = field.Remark,
                           Enable = field.Enable,
                           Order = field.Order,
                           Field = field.Field,
                           Feature = field.Feature,
                           Comment = field.Comment,
                           CreateUser = field.CreateUser,
                           SystemId = system.Oid,
                           SystemCode = system.Code,
                           SystemName = system.Name,
                           SystemType = system.Type,
                           Config = system.Config,
                           IP = system.IP,
                           Schema = system.Schema,
                           Solution = system.Solution,
                           TableId = table.Oid,
                           TableName = table.TableName,
                           TableComment = table.TableComment
                       };
            return linq.ToList();
        }

        /// <summary>
        /// 根据主键Id集合获取列表数据
        /// </summary>
        /// <param name="ids">主键Id集合</param>
        /// <returns></returns>
        public List<TaskFieldEntity> GetList(string[] ids)
        {
            var expression = ExtLinq.True<TaskFieldEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.FindList(expression);
        }
        #endregion
    }
    #endregion
}
