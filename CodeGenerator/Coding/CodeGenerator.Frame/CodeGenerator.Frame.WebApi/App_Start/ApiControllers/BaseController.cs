using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Xavier.Framework.Entity;

namespace CodeGenerator.Frame.WebApi.App_Start.ApiControllers
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class BaseController : ApiController
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="ajaxResult">ajax返回实体</param>
        /// <returns></returns>
        protected virtual AjaxResult Success(AjaxResult ajaxResult)
        {
            return ajaxResult;
        }

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="code">返回编码</param>
        /// <returns></returns>
        protected virtual AjaxResult Success(string message, string code = "0")
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                Code = code,
                Message = message,
                Success = true
            };
            return ajaxResult;
        }

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回值</param>
        /// <param name="code">返回编码</param>
        /// <returns></returns>
        protected virtual AjaxResult Success(string message, object data, string code = "0")
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                Code = code,
                Message = message,
                Success = true,
                Data = data
            };
            return ajaxResult;
        }

        /// <summary>
        /// 请求失败
        /// </summary>
        /// <param name="ajaxResult">ajax返回实体</param>
        /// <returns></returns>
        protected virtual AjaxResult Error(AjaxResult ajaxResult)
        {
            return ajaxResult;
        }

        /// <summary>
        /// 请求失败
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="code">返回编码</param>
        /// <returns></returns>
        protected virtual AjaxResult Error(string message, string code = "0")
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                Code = code,
                Message = message,
                Success = false
            };
            return ajaxResult;
        }

        /// <summary>
        /// 请求失败
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <param name="data">返回值</param>
        /// <param name="code">返回编码</param>
        /// <returns></returns>
        protected virtual AjaxResult Error(string message, object data, string code = "0")
        {
            AjaxResult ajaxResult = new AjaxResult
            {
                Code = code,
                Message = message,
                Success = false,
                Data = data
            };
            return ajaxResult;
        }
    }
}
