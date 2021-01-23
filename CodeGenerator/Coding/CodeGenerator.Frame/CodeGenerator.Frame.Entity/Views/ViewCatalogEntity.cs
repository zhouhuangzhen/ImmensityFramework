using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xavier.Framework.Db.Models;

namespace CodeGenerator.Frame.Entity.Views
{
    /// <summary>
    /// 数据库表视图实体--字典视图
    /// </summary>
    public class ViewCatalogEntity : BaseEntity<ViewCatalogEntity>
    {
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
        public int ItemOrder { get; set; }

        /// <summary>
        /// 字典明细所属分类
        /// </summary>
        public string ItemParentOid { get; set; }

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
        public int CatalogType { get; set; }

        /// <summary>
        /// 字典分类描述
        /// </summary>
        public string CatalogText { get; set; }
    }
}
