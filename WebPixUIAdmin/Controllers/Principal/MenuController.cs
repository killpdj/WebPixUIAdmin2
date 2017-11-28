using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers
{
    public class MenuController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: Menu
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarMenu/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            MenuViewModel[] Menus = jss.Deserialize<MenuViewModel[]>(result);

           // 

            return View(Menus);
        }

        // GET: Menu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarMenu/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            MenuViewModel[] menuViewModel = jss.Deserialize<MenuViewModel[]>(result);

            if (menuViewModel == null)
            {
                return HttpNotFound();
            }

            var MenusFiltrado = menuViewModel.Where(x => x.ID == id).ToList();

            return View(MenusFiltrado);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Url,Pai,Tipo,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Ativo,Status,idCliente")] MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                menuViewModel.idCliente = IDCliente;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarMenu/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Menu = menuViewModel };
                    var data = jss.Serialize(Envio);

                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(menuViewModel);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarMenu/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            MenuViewModel[] menuViewModel = jss.Deserialize<MenuViewModel[]>(result);

            if (menuViewModel == null)
            {
                return HttpNotFound();
            }

            var MenusFiltrado = menuViewModel.Where(x => x.ID == id).FirstOrDefault();

            return View(MenusFiltrado);
        }

        // POST: Menu/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Url,Pai,Tipo,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Ativo,Status,idCliente")] MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                menuViewModel.idCliente = IDCliente;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarMenu/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Menu = menuViewModel };
                    var data = jss.Serialize(Envio);

                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }
            return View(menuViewModel);
        }

        // GET: Menu/Delete/5
        public JsonResult Delete(int? id)
        {

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            var Envio = new { Estilo = new { idEstilo = id } };
            var data = jss.Serialize(Envio);
            try
            {
                using (var client = new WebClient())
                {
                    var url = keyUrl + "Seguranca/Principal/DeletarEstilo/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
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
               // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
