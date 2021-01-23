using System;

namespace Xavier.Framework.Db.Models
{
    /// <summary>
    /// 数据库表基础类
    /// </summary>
	public class BaseEntity
	{
        /// <summary>
        /// 数据唯一标识
        /// </summary>
		public string Oid
		{
			get;
			set;
		} = Guid.NewGuid().ToString().ToLower();
        
        /// <summary>
        /// 入库时间
        /// </summary>
		public DateTime InsertTime
		{
			get;
			set;
		} = DateTime.Now;

        /// <summary>
        /// 备注
        /// </summary>
		public string Remark
		{
			get;
			set;
		} = string.Empty;

        /// <summary>
        /// 是否可用
        /// </summary>
		public int Enable
		{
			get;
			set;
		} = 1;

	}
}
