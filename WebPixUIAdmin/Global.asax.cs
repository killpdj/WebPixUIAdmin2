using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebPixUIAdmin.Models;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest()
        {
            //teste 2
                string url = HttpContext.Current.Request.Url.Host;
                int porta = HttpContext.Current.Request.Url.Port;
                string protocolo = HttpContext.Current.Request.Url.Scheme;

                var urlDoCliente = "";

                if (porta != 80)
                    urlDoCliente = protocolo + "://" + url + ":" + porta.ToString() + "/";
                else
                    urlDoCliente = protocolo + "://" + url + "/";

                int idCliente = PixCore.PixCoreValues.VerificaUrlCliente(urlDoCliente);
                if (idCliente != 0)
                {
                    string cookievalue;
                    if (Request.Cookies["IdCliente"] != null)
                    {
                        cookievalue = Request.Cookies["IdCliente"].ToString();
                    }
                    else
                    {
                        Response.Cookies["IdCliente"].Value = idCliente.ToString();
                        Response.Cookies["IdCliente"].Expires = DateTime.Now.AddMinutes(1); // add expiry time
                    }
                    PixCore.PixCoreValues.RenderUrlPage(HttpContext.Current);
                }
                else
                {
                    Response.StatusCode = 404;
                }

                LoginViewModel usuariologado = PixCoreValues.UsuarioLogado;
                if (usuariologado == null || usuariologado.IdUsuario == 0)
                {
                    if (!HttpContext.Current.Request.Url.AbsoluteUri.Contains("login/login"))
                    {

                        //Verfica login
                        if (usuariologado == null || usuariologado.IdUsuario == 0)
                        {
                            HttpContext.Current.Response.Redirect(urlDoCliente + "login/login");
                        }
                        else
                            HttpContext.Current.Response.Redirect(urlDoCliente);

                    }
                }


        }
    }
}
