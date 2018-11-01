using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models;
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
            PageViewModel[] Pages = jss.Deserialize<PageViewModel[]>(result);

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
            var url = keyUrl + "Seguranca/Principal/buscarpaginas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] pageViewModel = jss.Deserialize<PageViewModel[]>(result);

            if (pageViewModel == null)
            {
                return HttpNotFound();
            }
            return View(pageViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // GET: Page/Create
        public ActionResult Create()
        {
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
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarpagina/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { page = pageViewModel };
                    var data = jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(pageViewModel);
        }

        // GET: Page/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarpaginas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] pageViewModel = jss.Deserialize<PageViewModel[]>(result);

            if (pageViewModel == null)
            {
                return HttpNotFound();
            }
            return View(pageViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        public ActionResult EditorDynamic(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarpaginas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PageViewModel[] pageViewModel = jss.Deserialize<PageViewModel[]>(result);

            if (pageViewModel == null)
            {
                return HttpNotFound();
            }
            return View(pageViewModel.Where(z => z.ID == id).FirstOrDefault());
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
          //  var pagina = pageViewModel.Conteudo;

          //  pageViewModel.Conteudo //= pagina;

            if (ModelState.IsValid)
            {
                var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                var Envio = new { page = pageViewModel };
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
                    return Json(new { msg = result },JsonRequestBehavior.AllowGet);
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
