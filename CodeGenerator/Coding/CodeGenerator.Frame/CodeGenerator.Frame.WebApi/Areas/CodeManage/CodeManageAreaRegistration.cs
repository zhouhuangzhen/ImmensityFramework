using System.Web.Mvc;

namespace CodeGenerator.Frame.WebApi.Areas.CodeManage
{
    /// <summary>
    /// 区域--代码管理
    /// </summary>
    public class CodeManageAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "CodeManage";
            }
        }
        
        /// <summary>
        /// 区域注册
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CodeManage_default",
                "CodeManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}