using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Integracao_Google_API_DOTNET.Startup))]
namespace Integracao_Google_API_DOTNET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
