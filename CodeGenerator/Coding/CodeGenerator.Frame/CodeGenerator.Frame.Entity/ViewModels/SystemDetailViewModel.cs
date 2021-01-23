using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Entity.ViewModels
{
    /// <summary>
    /// 视图模型--系统明细配置
    /// </summary>
    public class SystemDetailViewModel
    {
        /// <summary>
        /// 数据唯一标识
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int Enable { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 系统Id
        /// </summary>
        public string SystemId { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 数据库类型(DatabaseType : 1:Oracle , 2:MySQL , 3:Oracle , 4:SQLite , 5:Postgre)
        /// </summary>
        public int SystemType { get; set; }

        /// <summary>
        /// 解决方案名称
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 数据库表Schema
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 字典分类编码
        /// </summary>
        public string CatalogCode { get; set; }

        /// <summary>
        /// 字典分类名称
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public string CatalogType { get; set; }

        /// <summary>
        /// 字典分类描述
        /// </summary>
        public string CatalogText { get; set; }

        /// <summary>
        /// 字典明细编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 字典明细名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 字典明细值
        /// </summary>
        public string ItemValue { get; set; }

        /// <summary>
        /// 字典明细描述
        /// </summary>
        public string ItemText { get; set; }

        /// <summary>
        /// 字典明细排序
        /// </summary>
        public string ItemOrder { get; set; }

        /// <summary>
        /// 字典明细所属分类
        /// </summary>
        public string ItemParentOid { get; set; }
    }
}
