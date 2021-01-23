namespace Xavier.Framework.Entity
{
    /// <summary>
    /// 前端响应返回实体
    /// </summary>
	public class AjaxResult
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AjaxResult()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
		public AjaxResult(bool success)
        {
            Success = success;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
		public AjaxResult(bool success, string message)
            : this(success)
        {
            Message = message;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        /// <param name="data">返回内容</param>
		public AjaxResult(bool success, string message, object data)
            : this(success, message)
        {
            Data = data;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        /// <param name="data">返回内容</param>
        /// <param name="code">编码</param>
		public AjaxResult(bool success, string message, object data, string code)
            : this(success, message, data)
        {
            Code = code;
        }
        #endregion

        #region 字段
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 返回消息
        /// </summary>
		public string Message { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
		public object Data { get; set; }
        #endregion

    }
}
