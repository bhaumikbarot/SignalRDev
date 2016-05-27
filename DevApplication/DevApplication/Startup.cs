using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevApplication.Startup))]
namespace DevApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            var hubConfiguration = new HubConfiguration { Resolver = new DefaultDependencyResolver() };
            hubConfiguration.EnableDetailedErrors = true;
            GlobalHost.Configuration.DefaultMessageBufferSize = 2000;
            app.MapSignalR(hubConfiguration);
        }
    }
}
