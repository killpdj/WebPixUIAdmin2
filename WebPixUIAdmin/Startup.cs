using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPixUIAdmin.Startup))]
namespace WebPixUIAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
