using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Dependences;
using DiySoccer.Core.Attributes;

namespace DiySoccer
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule(new AutofacModule());

            builder.RegisterFilterProvider();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            builder.RegisterType<DiySoccerAuthorizeAttribute>().PropertiesAutowired();

            var container = builder.Build();
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}