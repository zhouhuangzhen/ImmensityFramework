namespace Xavier.Framework.Db.Models
{
    /// <summary>
    /// 分页类
    /// </summary>
	public class Pagination
	{
        /// <summary>
        /// 每页行数
        /// </summary>
		public int rows
		{
			get;
			set;
		} = 10;

        /// <summary>
        /// 当前页
        /// </summary>
		public int page
		{
			get;
			set;
		} = 1;

        /// <summary>
        /// 排序字段，多个字段用,分割,排序方式与字段直接用空格分割，如果只有字段无空格，默认排序为sord字段值
        /// </summary>
		public string sidx
		{
			get;
			set;
		} = "InsertTime";

        /// <summary>
        /// 排序方式
        /// </summary>
		public string sord
		{
			get;
			set;
		} = "asc";


        /// <summary>
        /// 总记录数
        /// </summary>
		public int records
		{
			get;
			set;
		}

        /// <summary>
        /// 总页数
        /// </summary>
		public int total
		{
			get
			{
				if (records > 0)
				{
					return (records % rows == 0) ? (records / rows) : (records / rows + 1);
				}
				return 0;
			}
		}
	}
}
