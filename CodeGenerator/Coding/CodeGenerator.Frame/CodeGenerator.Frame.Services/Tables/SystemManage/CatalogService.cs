using CodeGenerator.Frame.Dals.Tables.SystemManage;
using CodeGenerator.Frame.Entity.Tables.SystemManage;
using CodeGenerator.Frame.IServices.Tables.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Services.Tables.SystemManage
{
    /// <summary>
    /// 业务处理类--字典分类表
    /// </summary>
    public class CatalogService : ICatalogService,IDisposable
    {
        /// <summary>
        /// 数据库处理类--字典分类表
        /// </summary>
        private readonly CatalogDal dalCatalog = new CatalogDal();

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            this.dalCatalog.Dispose();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        public List<CatalogEntity> GetList(string code, string name, int type)
        {
            try
            {
                return dalCatalog.GetList(code, name, type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
        }
    }
}
