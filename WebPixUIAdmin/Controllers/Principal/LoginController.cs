using System;
using System.Web.Mvc;
using WebPixUIAdmin.Models;

namespace WebPixUIAdmin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login/Create
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Login(LoginViewModel collection)
        {
            try
            {
                if (PixCore.PixCoreValues.Login(collection))
                {
                    Response.Redirect(PixCore.PixCoreValues.defaultSiteUrl);
                    return View();
                }
                else
                {
                    ViewBag.TemporariaMensagem = "Usuario ou senha invalida";
                    Response.Write("Usuario ou senha invalida");
                    return View();
                }
            }
            catch(Exception e)
            {
                ViewBag.TemporariaMensagem = "Usuario ou senha invalida";
                Response.Write(e.InnerException);
                return View();
            }
        }
    }
}
