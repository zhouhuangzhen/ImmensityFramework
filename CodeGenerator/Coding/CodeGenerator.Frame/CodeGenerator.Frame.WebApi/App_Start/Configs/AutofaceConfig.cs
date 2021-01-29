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
            //得到你的HttpConfiguration.
            var configuration = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();

            //注册控制器
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            //可选：注册Autofac过滤器提供商.
            builder.RegisterWebApiFilterProvider(configuration);


            //builder.RegisterType(typeof(MyAuthenticationAttribute)).PropertiesAutowired(); //单独注册我们的MyAuthenticationAttribute过滤器
            var webapiAssembly = Assembly.Load("CodeGenerator.Frame.WebApi");

            //注册webapi项目中现实了IAuthorizationFilter接口或者实现了IActionFilter接口的非抽象过滤器类
            builder.RegisterAssemblyTypes(webapiAssembly).Where(r => !r.IsAbstract &&
            (typeof(IAuthorizationFilter).IsAssignableFrom(r)) || typeof(IActionFilter).IsAssignableFrom(r)).PropertiesAutowired();


            //对Repositorys这个程序集实现了IBaseRepository接口的非抽象类进行注册
            var repository = Assembly.Load("CodeGenerator.Frame.Services");
            builder.RegisterAssemblyTypes(repository).Where(r => !r.IsAbstract).AsImplementedInterfaces().SingleInstance()
               .PropertiesAutowired();


            IContainer container = builder.Build();
            //将依赖关系解析器设置为Autofac。
            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = resolver;




            ////  创建容器
            //var builder = new ContainerBuilder();

            ////  Get your HttpConfiguration.
            //var config = GlobalConfiguration.Configuration;

            ////  注册所有Controller
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            ////  所有引用的程序集
            //var referencesAseemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();

            ////  注册所有Service
            //builder.RegisterAssemblyTypes(referencesAseemblies.Where(x => x.FullName.StartsWith("CodeGenerator.")).ToArray())
            //    .Where(t => t.Name.EndsWith("Services") && !t.IsAbstract)
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();

            //var container = builder.Build();    //Build方法创建一个容器
            //var resolve = new AutofacWebApiDependencyResolver(container);
            ////注册MVC容器
            //DependencyResolver.SetResolver(resolve);       
            ////  注册api容器需要使用HttpConfiguration对象
            //config.DependencyResolver = resolve;
        }
    }
}