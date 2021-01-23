namespace Xavier.Framework.Entity
{
    /// <summary>
    /// ǰ����Ӧ����ʵ��
    /// </summary>
	public class AjaxResult
    {
        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public AjaxResult()
        { }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="success">�Ƿ�ɹ�</param>
		public AjaxResult(bool success)
        {
            Success = success;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="success">�Ƿ�ɹ�</param>
        /// <param name="message">��Ϣ</param>
		public AjaxResult(bool success, string message)
            : this(success)
        {
            Message = message;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="success">�Ƿ�ɹ�</param>
        /// <param name="message">��Ϣ</param>
        /// <param name="data">��������</param>
		public AjaxResult(bool success, string message, object data)
            : this(success, message)
        {
            Data = data;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="success">�Ƿ�ɹ�</param>
        /// <param name="message">��Ϣ</param>
        /// <param name="data">��������</param>
        /// <param name="code">����</param>
		public AjaxResult(bool success, string message, object data, string code)
            : this(success, message, data)
        {
            Code = code;
        }
        #endregion

        #region �ֶ�
        /// <summary>
        /// ����
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// �Ƿ�ɹ�
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// ������Ϣ
        /// </summary>
		public string Message { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
		public object Data { get; set; }
        #endregion

    }
}
