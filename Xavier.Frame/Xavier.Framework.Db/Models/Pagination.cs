namespace Xavier.Framework.Db.Models
{
    /// <summary>
    /// ��ҳ��
    /// </summary>
	public class Pagination
	{
        /// <summary>
        /// ÿҳ����
        /// </summary>
		public int rows
		{
			get;
			set;
		} = 10;

        /// <summary>
        /// ��ǰҳ
        /// </summary>
		public int page
		{
			get;
			set;
		} = 1;

        /// <summary>
        /// �����ֶΣ�����ֶ���,�ָ�,����ʽ���ֶ�ֱ���ÿո�ָ���ֻ���ֶ��޿ո�Ĭ������Ϊsord�ֶ�ֵ
        /// </summary>
		public string sidx
		{
			get;
			set;
		} = "InsertTime";

        /// <summary>
        /// ����ʽ
        /// </summary>
		public string sord
		{
			get;
			set;
		} = "asc";


        /// <summary>
        /// �ܼ�¼��
        /// </summary>
		public int records
		{
			get;
			set;
		}

        /// <summary>
        /// ��ҳ��
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
