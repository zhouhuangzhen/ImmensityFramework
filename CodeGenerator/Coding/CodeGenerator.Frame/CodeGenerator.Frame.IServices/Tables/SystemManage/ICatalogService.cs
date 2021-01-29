using CodeGenerator.Frame.Entity.Tables.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.IServices.Tables.SystemManage
{
    /// <summary>
    /// 业务处理接口--字典分类表
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="name">名称</param>
        /// <param name="type">字典类型</param>
        /// <returns></returns>
        List<CatalogEntity> GetList(string code, string name, int type);
    }
}
