using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers
{
    public class UsuarioController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: Usuario
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarUsuario/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            UsuarioViewModel[] Usuarios = jss.Deserialize<UsuarioViewModel[]>(result);

            var usuarioFiltrado = Usuarios.Where(x => x.idCliente == IDCliente).ToList();

            return View(usuarioFiltrado);
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarUsuario/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            UsuarioViewModel[] Usuarios = jss.Deserialize<UsuarioViewModel[]>(result);


            if (Usuarios == null)
            {
                return HttpNotFound();
            }

            var usuarioFiltrado = Usuarios.Where(x => x.ID == id).FirstOrDefault();

            return View(usuarioFiltrado);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Login,SobreNome,Email,PerfilUsuario,Senha,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Ativo,Status,idCliente")] UsuarioViewModel usuarioViewModel)
        {
            if (ModelState.IsValid)
            {

                usuarioViewModel.idCliente = IDCliente;
                usuarioViewModel.UsuarioEdicao = 1;
                usuarioViewModel.UsuarioCriacao = 1;
                using (var client = new WebClient())
                {

                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "usuario";
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var data = jss.Serialize(usuarioViewModel);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(usuarioViewModel);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarUsuario/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            UsuarioViewModel[] Usuarios = jss.Deserialize<UsuarioViewModel[]>(result);


            if (Usuarios == null)
            {
                return HttpNotFound();
            }

            var usuarioFiltrado = Usuarios.Where(x => x.ID == id).FirstOrDefault();

            return View(usuarioFiltrado);
        }

        // POST: Usuario/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Login,SobreNome,Email,PerfilUsuario,Senha,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Ativo,Status,idCliente")] UsuarioViewModel usuarioViewModel)
        {
            if (ModelState.IsValid)
            {
                usuarioViewModel.idCliente = IDCliente;
                usuarioViewModel.UsuarioEdicao = 1;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarUsuario/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();

                    var Envio = new { usuario = usuarioViewModel };
                    var data = jss.Serialize(Envio);

                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(usuarioViewModel);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "usuario" + "/" + id;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            UsuarioViewModel Usuarios = jss.Deserialize<UsuarioViewModel>(result);

            if (Usuarios == null)
            {
                return HttpNotFound();
            }
            return View(Usuarios);
        }

        // POST: Usuario/Delete/5
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
