using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalRService.Startup))]
namespace SignalRService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
