using System.Web.Mvc;

namespace CodeGenerator.Frame.WebApi.Areas.SystemManage
{
    /// <summary>
    /// 区域--系统管理
    /// </summary>
    public class SystemManageAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "SystemManage";
            }
        }

        /// <summary>
        /// 区域注册
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SystemManage_default",
                "SystemManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}