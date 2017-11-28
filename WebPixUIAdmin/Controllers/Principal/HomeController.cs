using System;
using System.Web.Mvc;

namespace WebPixUIAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logout()
        {            
            Response.Cookies["UsuarioLogado"].Value = "";
            Response.Cookies["UsuarioLogado"].Expires = DateTime.Now;

            return RedirectToAction("Login", "Login");
        }
    }
}