using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models;

namespace WebPixUIAdmin.Controllers
{
    public class PerfilController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: Perfil
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "perfil/GetAllPerfil/"+IDCliente;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PerfilViewModel[] Perfil = jss.Deserialize<PerfilViewModel[]>(result);

           

            return View(Perfil);
        }

        // GET: Perfil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "perfil/GetAllPerfil/" + IDCliente;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PerfilViewModel[] perfilViewModel = jss.Deserialize<PerfilViewModel[]>(result);

            if (perfilViewModel == null)
            {
                return HttpNotFound();
            }

            var PerfilFiltrado = perfilViewModel.Where(x => x.ID == id).FirstOrDefault();

            return View(perfilViewModel);
        }

        // GET: Perfil/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Perfil/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Ativo,Status,idCliente")] PerfilViewModel perfilViewModel)
        {
            if (ModelState.IsValid)
            {
                perfilViewModel.idCliente = IDCliente;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "perfil/SavePerfil";
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var data = jss.Serialize(perfilViewModel);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(perfilViewModel);
        }

        // GET: Perfil/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "perfil/GetAllPerfil/" + IDCliente;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PerfilViewModel[] perfilViewModel = jss.Deserialize<PerfilViewModel[]>(result);

            if (perfilViewModel == null)
            {
                return HttpNotFound();
            }

            var PerfilFiltrado = perfilViewModel.Where(x => x.ID == id).FirstOrDefault();

            return View(perfilViewModel);
        }

        // POST: Perfil/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Ativo,Status,idCliente")] PerfilViewModel perfilViewModel)
        {
            if (ModelState.IsValid)
            {
                perfilViewModel.idCliente = IDCliente;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "perfil/SavePerfil";
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var data = jss.Serialize(perfilViewModel);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }
            return View(perfilViewModel);
        }

        // GET: Perfil/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "perfil/" + id;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PerfilViewModel perfilViewModel = jss.Deserialize<PerfilViewModel>(result);

            if (perfilViewModel == null)
            {
                return HttpNotFound();
            }
            return View(perfilViewModel);
        }

        // POST: Perfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
              //  db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
