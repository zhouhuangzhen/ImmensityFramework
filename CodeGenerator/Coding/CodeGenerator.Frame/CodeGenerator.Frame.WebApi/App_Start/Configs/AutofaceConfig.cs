using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;

namespace CodeGenerator.Frame.WebApi.App_Start.Configs
{
    /// <summary>
    /// Autofac依赖注入配置
    /// </summary>
    public class AutofaceConfig
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        public static void RegisterDependencies()
        {
            SetAutofacWebapi();
        }

        private static void SetAutofacWebapi()
        {
            //  创建容器
            var builder = new ContainerBuilder();
            
            //  Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            //  注册所有Controller
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //  所有引用的程序集
            var referencesAseemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();

            //  注册所有Service
            builder.RegisterAssemblyTypes(referencesAseemblies.ToArray())
                .Where(t => t.Name.EndsWith("Services") && !t.IsAbstract)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var container = builder.Build();    //Build方法创建一个容器
            DependencyResolver.SetResolver(new AutofacWebApiDependencyResolver(container));       //注册MVC容器
        }
    }
}