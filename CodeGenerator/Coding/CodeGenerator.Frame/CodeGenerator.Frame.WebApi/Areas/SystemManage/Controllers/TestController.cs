using CodeGenerator.Frame.WebApi.App_Start.ApiControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Xavier.Framework.Entity;

namespace CodeGenerator.Frame.WebApi.Areas.SystemManage.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : BaseController
    {
        [HttpGet, Route("xxx")]
        public AjaxResult Get()
        {
            return Success("我是自定义路由Action，Get方法");
        }

        public AjaxResult Post()
        {
            return Success("我是自定义路由Action，Post方法");
        }

        [HttpDelete]
        public AjaxResult Delete()
        {
            return Success("我是自定义路由Action，Delete方法");
        }

        [HttpPut]
        public AjaxResult Put()
        {
            return Success("我是自定义路由Action，Put方法");
        }
    }
}
