using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CodeGenerator.Frame.WebApi.App_Start.Configs
{
    /// <summary>
    /// 日志注册类
    /// </summary>
    public class LogConfig
    {
        /// <summary>
        /// 注册日志
        /// </summary>
        public static void Register()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}