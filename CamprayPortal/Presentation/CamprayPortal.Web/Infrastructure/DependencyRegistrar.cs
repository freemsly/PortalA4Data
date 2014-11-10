using Autofac;
using Autofac.Core;
using CamprayPortal.Core.Caching;
using CamprayPortal.Core.Infrastructure;
using CamprayPortal.Core.Infrastructure.DependencyManagement;
using CamprayPortal.Web.Controllers;
using CamprayPortal.Web.Infrastructure.Installation;

namespace CamprayPortal.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //we cache presentation models between requests
           
            builder.RegisterType<CountryController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
            builder.RegisterType<CommonController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
            builder.RegisterType<NewsController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
              
            builder.RegisterType<TopicController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
            builder.RegisterType<WidgetController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
            
            //installation localization service
            builder.RegisterType<InstallationLocalizationService>().As<IInstallationLocalizationService>().InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
