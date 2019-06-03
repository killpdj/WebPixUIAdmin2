using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using WebPixUIAdmin.Helper;
using WebPixUIAdmin.Models;
using WebPixUIAdmin.Models.Principal;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers
{
    public class PageController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: Page
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarpaginas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] pageView = jss.Deserialize<PageViewModel[]>(result);
            List<PageViewModel> Pages = new List<PageViewModel>();

            foreach (PageViewModel page in pageView)
            {
                byte[] report = Convert.FromBase64String(page.Conteudo);
                page.Conteudo = Encoding.UTF8.GetString(report);
                Pages.Add(page);
            }
                
            var PagesFiltrado = Pages.Where(x => x.idCliente == IDCliente).ToList();

            return View(PagesFiltrado);
        }

        // GET: Page/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "seguranca/Principal/BuscarTemas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));

            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] pageView = jss.Deserialize<PageViewModel[]>(result);
            List<PageViewModel> Pages = new List<PageViewModel>();

            if (pageView == null)
            {
                return HttpNotFound();
            }

            foreach (PageViewModel page in pageView)
            {
                byte[] report = Convert.FromBase64String(page.Conteudo);
                page.Conteudo = Encoding.UTF8.GetString(report);
                Pages.Add(page);
            }


            return View(Pages.Where(z => z.ID == id).FirstOrDefault());
        }

        // GET: Page/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateHelper()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/BuscarTemas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            TemasViewModel[] temasViewModel = jss.Deserialize<TemasViewModel[]>(result);
            ViewBag.Conteudo = temasViewModel.FirstOrDefault().Conteudo;
            return View();
        }
        public ActionResult EditorHelper(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == 0)
            {
                PageViewModel obj = Session["Helper"] as PageViewModel;
                return View(obj);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarpaginas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] pageView = jss.Deserialize<PageViewModel[]>(result);
            List<PageViewModel> Pages = new List<PageViewModel>();

            if (pageView == null)
            {
                return HttpNotFound();
            }

            foreach (PageViewModel page in pageView)
            {
                byte[] report = Convert.FromBase64String(page.Conteudo);
                page.Conteudo = Encoding.UTF8.GetString(report);
                Pages.Add(page);
            }

            var retorno = Pages.Where(x => x.ID == id).FirstOrDefault();


            ViewBag.Conteudo = retorno.Conteudo;
            ViewBag.ID = retorno.ID;
            return View();
        }


        // POST: Page/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Titulo,Conteudo,Url,Nome,Descricao,Ativo,Status")] PageViewModel pageViewModel)
        {

            if (ModelState.IsValid)
            {
                pageViewModel.DataCriacao = Convert.ToDateTime("01/08/1993");
                pageViewModel.DateAlteracao = Convert.ToDateTime("01/08/1993");
                pageViewModel.idCliente = IDCliente;

                PageModel pageSend = ConvertsHelper.ConvertToPageModel(pageViewModel);

                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarpagina/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { page = pageSend };
                    var data = jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(pageViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult CreateForHelp(String conteudo)
        {

            PageViewModel page = new PageViewModel();
            page.Conteudo = conteudo;

            Session["Helper"] = page;
            return Json(new { msg = "deu certo" } ,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditorForHelp(String conteudo)
        {

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarpaginas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] pageView = jss.Deserialize<PageViewModel[]>(result);
            List<PageViewModel> Pages = new List<PageViewModel>();

            int id = Convert.ToInt32(Request.QueryString["id"]);
            

            foreach (PageViewModel page in pageView)
            {
                byte[] report = Convert.FromBase64String(page.Conteudo);
                page.Conteudo = Encoding.UTF8.GetString(report);
                Pages.Add(page);
            }

            var retorno = Pages.Where(x => x.ID == id).FirstOrDefault();
            retorno.Conteudo = conteudo;

            Session["Helper"] = retorno;
            Session["Editor"] = true;
            return Json(new { msg = "/Page/Edit/" + id }, JsonRequestBehavior.AllowGet);
        }

        // GET: Page/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id == 0)
            {
                PageViewModel obj = Session["Helper"] as PageViewModel;
                return View(obj);
            }
            if (Session["Editor"] != null)
            {
                if (Convert.ToBoolean(Session["Editor"]))
                {
                    PageViewModel obj = Session["Helper"] as PageViewModel;
                    return View(obj);
                }
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarpaginas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] pageView = jss.Deserialize<PageViewModel[]>(result);
            List<PageViewModel> Pages = new List<PageViewModel>();

            if (pageView == null)
            {
                return HttpNotFound();
            }

            foreach (PageViewModel page in pageView)
            {
                byte[] report = Convert.FromBase64String(page.Conteudo);
                page.Conteudo = Encoding.UTF8.GetString(report);
                Pages.Add(page);
            }


            return View(Pages.Where(z => z.ID == id).FirstOrDefault());
        }

        // POST: Page/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Titulo,Conteudo,Url,Nome,Descricao,Ativo,Status")] PageViewModel pageViewModel)
        {
            pageViewModel.UsuarioEdicao = 1;
            pageViewModel.idCliente = IDCliente;
            if(pageViewModel.ID == 0)
            {
                if (ModelState.IsValid)
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    pageViewModel.DataCriacao = Convert.ToDateTime("01/08/1993");
                    pageViewModel.DateAlteracao = Convert.ToDateTime("01/08/1993");
                    pageViewModel.idCliente = IDCliente;

                    PageModel pageSend  = ConvertsHelper.ConvertToPageModel(pageViewModel);

                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { page = pageSend };
                    var data = jss.Serialize(Envio);

                    using (var client = new WebClient())
                    {
                        var url = keyUrl + "Seguranca/Principal/salvarpagina/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        var result = client.UploadString(url, "POST", data);
                    }
                    return RedirectToAction("Index");
                }
            }
            if (ModelState.IsValid)
            {

                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();

                PageModel pageSend = ConvertsHelper.ConvertToPageModel(pageViewModel);

                var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                var Envio = new { page = pageSend };
                var data = jss.Serialize(Envio);

                using (var client = new WebClient())
                {
                    var url = keyUrl + "Seguranca/Principal/salvarpagina/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");              
            }
            return View(pageViewModel);
        }



        // GET: Page/Delete/5
        public JsonResult Delete(int? id)
        {

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            var Envio = new { idPagina = new { idPagina = id } };
            var data = jss.Serialize(Envio);
            try
            {
                using (var client = new WebClient())
                {
                    var url = keyUrl + "Seguranca/Principal/DeletarPagina/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var result = client.UploadString(url, "POST", data);
                    return Json(new { msg = result }, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json(new { msg = "Houve um erro tente novamente mais tarde" }, JsonRequestBehavior.AllowGet);
            }


        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
