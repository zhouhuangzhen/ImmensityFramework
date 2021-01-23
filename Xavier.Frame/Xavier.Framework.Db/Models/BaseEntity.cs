using System;

namespace Xavier.Framework.Db.Models
{
    /// <summary>
    /// ���ݿ�������
    /// </summary>
	public class BaseEntity
	{
        /// <summary>
        /// ����Ψһ��ʶ
        /// </summary>
		public string Oid
		{
			get;
			set;
		} = Guid.NewGuid().ToString().ToLower();
        
        /// <summary>
        /// ���ʱ��
        /// </summary>
		public DateTime InsertTime
		{
			get;
			set;
		} = DateTime.Now;

        /// <summary>
        /// ��ע
        /// </summary>
		public string Remark
		{
			get;
			set;
		} = string.Empty;

        /// <summary>
        /// �Ƿ����
        /// </summary>
		public int Enable
		{
			get;
			set;
		} = 1;

	}
}
