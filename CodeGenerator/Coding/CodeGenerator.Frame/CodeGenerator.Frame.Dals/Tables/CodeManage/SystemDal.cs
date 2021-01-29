using CodeGenerator.Frame.Entity.Tables.CodeManage;
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
    #region 数据库处理类--系统信息表
    /// <summary>
    /// 数据库处理类--系统信息表
    /// </summary>
    public class SystemDal
    {
        #region 变量
        /// <summary>
        /// 数据库处理对象--系统信息表
        /// </summary>
        private readonly IRepositoryBase<SystemEntity> baseRepository = new RepositoryBase<SystemEntity>();
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
        public int Insert(SystemEntity entity)
        {
            return baseRepository.Insert(entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        /// <returns></returns>
        public int Insert(List<SystemEntity> entities)
        {
            return baseRepository.Insert(entities);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entities">新增实体集</param>
        public void InsertBulk(List<SystemEntity> entities)
        {
            baseRepository.BulkInsert(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新</param>
        /// <returns></returns>
        public int Update(SystemEntity entity)
        {
            return baseRepository.Update(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        /// <returns></returns>
        public int Update(List<SystemEntity> entities)
        {
            return baseRepository.Update(entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities">更新实体集</param>
        public void UpdateBulk(List<SystemEntity> entities)
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
            var expression = ExtLinq.True<SystemEntity>();
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
            var expression = ExtLinq.True<SystemEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.Delete(expression);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">删除实体</param>
        /// <returns></returns>
        public int Delete(SystemEntity entity)
        {
            return baseRepository.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        /// <returns></returns>
        public int Delete(List<SystemEntity> entities)
        {
            return baseRepository.Delete(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities">删除实体集</param>
        public void DeleteBulk(List<SystemEntity> entities)
        {
            baseRepository.BulkDelete(entities);
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public SystemEntity GetEntity(string id)
        {
            return baseRepository.FindEntity(id);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="code">系统编码</param>
        /// <param name="name">系统名称</param>
        /// <param name="type">数据库类型</param>
        /// <param name="ip">服务器Ip地址</param>
        /// <param name="solution">解决方案名称</param>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        public List<SystemEntity> GetPaged(string code, string name, int type, string ip, string solution, Pagination pagination)
        {
            var expression = ExtLinq.True<SystemEntity>();
            if (!string.IsNullOrWhiteSpace(code))
            {
                expression = expression.And(x => x.Code.Contains(code));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                expression = expression.And(x => x.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(ip))
            {
                expression = expression.And(x => x.IP.Contains(ip));
            }
            if (!string.IsNullOrWhiteSpace(solution))
            {
                expression = expression.And(x => x.Solution.Contains(solution));
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
        /// <param name="code">系统编码</param>
        /// <param name="name">系统名称</param>
        /// <param name="type">数据库类型</param>
        /// <param name="ip">服务器Ip地址</param>
        /// <param name="solution">解决方案名称</param>
        /// <returns></returns>
        public List<SystemEntity> GetList(string code, string name, int type, string ip, string solution, Pagination pagination)
        {
            var expression = ExtLinq.True<SystemEntity>();
            if (!string.IsNullOrWhiteSpace(code))
            {
                expression = expression.And(x => x.Code.Contains(code));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                expression = expression.And(x => x.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(ip))
            {
                expression = expression.And(x => x.IP.Contains(ip));
            }
            if (!string.IsNullOrWhiteSpace(solution))
            {
                expression = expression.And(x => x.Solution.Contains(solution));
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
        public List<SystemEntity> GetList(string[] ids)
        {
            var expression = ExtLinq.True<SystemEntity>();
            expression = expression.And(x => ids.Contains(x.Oid));
            return baseRepository.FindList(expression);
        }
        #endregion
    }
    #endregion
}
