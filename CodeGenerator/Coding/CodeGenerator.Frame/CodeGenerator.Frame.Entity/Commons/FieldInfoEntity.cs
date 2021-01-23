using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Frame.Entity.Commons
{
    /// <summary>
    /// 系统数据库表字段实体
    /// </summary>
    public class FieldInfoEntity
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表类型
        /// </summary>
        public string TableType { get; set; }

        /// <summary>
        /// 字段排序
        /// </summary>
        public int Orders { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string ColumnDesc { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType { get; set; }
        
        /// <summary>
        /// 字段总长度
        /// </summary>
        public int TotalLength { get; set; }

        /// <summary>
        /// 浮点数长度
        /// </summary>
        public int FloatLength { get; set; }

        /// <summary>
        /// 是否自增(1：是，0：否)
        /// </summary>
        public int Logo { get; set; }

        /// <summary>
        /// 是否主键(1：是，0：否)
        /// </summary>
        public int PrimaryKey { get; set; }

        /// <summary>
        /// 是否可空(0:不可空，1：可空)
        /// </summary>
        public int IsEmpty { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Defaults { get; set; }
    }
}
