using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models.Produto;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers.MotoresAux
{
    public class PropiedadesController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: Propiedades
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarpropiedades/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoSkuViewModel[] Propiedades = jss.Deserialize<ProdutoSkuViewModel[]>(result);

            var PropiedadesFiltrado = Propiedades.Where(x => x.idCliente == IDCliente).ToList();

            return View(PropiedadesFiltrado);
        }

        // GET: Propiedades/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarpropiedades/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoSkuViewModel[] propiedadesViewModel = jss.Deserialize<ProdutoSkuViewModel[]>(result);

            if (propiedadesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(propiedadesViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // GET: Propiedades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propiedades/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ativo,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Status,idCliente")] PropiedadesViewModel propiedadesViewModel)
        {
            if (ModelState.IsValid)
            {
                propiedadesViewModel.DataCriacao = DateTime.Now;
                propiedadesViewModel.DateAlteracao = DateTime.Now;
                propiedadesViewModel.idCliente = IDCliente;
                propiedadesViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                propiedadesViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;

                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Produto/salvarpropiedades/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Propiedades = propiedadesViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(propiedadesViewModel);
        }

        // GET: Propiedades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarpropiedades/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoSkuViewModel[] propiedadesViewModel = jss.Deserialize<ProdutoSkuViewModel[]>(result);

            if (propiedadesViewModel == null)
            {
                return HttpNotFound();
            }
            return View(propiedadesViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // POST: Propiedades/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ativo,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Status,idCliente")] PropiedadesViewModel propiedadesViewModel)
        {
            if (ModelState.IsValid)
            {
                propiedadesViewModel.DataCriacao = DateTime.Now;
                propiedadesViewModel.DateAlteracao = DateTime.Now;
                propiedadesViewModel.idCliente = IDCliente;
                propiedadesViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                propiedadesViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;

                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Produto/salvarpropiedades/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Propiedades = propiedadesViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }
            return View(propiedadesViewModel);
        }

        // GET: Propiedades/Delete/5
        public ActionResult Delete(int? id)
        {
           
            return View();
        }

        // POST: Propiedades/Delete/5
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
               // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
