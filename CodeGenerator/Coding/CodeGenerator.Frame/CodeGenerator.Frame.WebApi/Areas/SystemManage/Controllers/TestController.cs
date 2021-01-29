using CodeGenerator.Frame.IServices.Tables.SystemManage;
using CodeGenerator.Frame.WebApi.App_Start.ApiControllers;
using log4net;
using Newtonsoft.Json;
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
        private readonly ICatalogService serviceCatalog = null;

        private readonly ILog logger = LogManager.GetLogger(typeof(TestController));

        public TestController(ICatalogService _serviceCatalog)
        {
            this.serviceCatalog = _serviceCatalog;
        }

        [HttpGet, Route("xxx")]
        public AjaxResult Get()
        {
            logger.Error("xxx");
            var res = this.serviceCatalog.GetList("", "", -1);
            return Success("我是自定义路由Action，Get方法 ： " + JsonConvert.SerializeObject(res));
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
