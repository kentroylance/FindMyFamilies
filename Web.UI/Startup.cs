using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FindMyFamilies.Startup))]
namespace FindMyFamilies
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string test = "test";
        }
    }
}
