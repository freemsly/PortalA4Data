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
           

            
            //installation localization service
            builder.RegisterType<InstallationLocalizationService>().As<IInstallationLocalizationService>().InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 2; }
        }
    }
}
