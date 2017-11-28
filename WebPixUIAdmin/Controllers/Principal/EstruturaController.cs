using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models;
using WebPixUIAdmin.Models.Principal;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers.Principal
{
    public class EstruturaController : Controller
    {
        private WebPixUIAdminContext db = new WebPixUIAdminContext();
        private int IDCliente = PixCore.PixCoreValues.IDCliente;
        // GET: Estrutura
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarEstruturas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            EstruturaViewModel[] Estrutura = jss.Deserialize<EstruturaViewModel[]>(result);

            var EstruturaFiltrado = Estrutura.Where(x => x.idCliente == IDCliente).ToList();
            return View(EstruturaFiltrado);
        }

        // GET: Estrutura/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarEstruturas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            EstruturaViewModel[] estruturaViewModel = jss.Deserialize<EstruturaViewModel[]>(result);

            if (estruturaViewModel == null)
            {
                return HttpNotFound();
            }
            return View(estruturaViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // GET: Estrutura/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Estrutura/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ativo,Tipo,idPermissao,idPai,Ativo,Principal,Imagem,ImagemMenu,LinkManual,UrlManual,Ordem,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Status,idCliente")] EstruturaViewModel estruturaViewModel)
        {
            if (ModelState.IsValid)
            {
                estruturaViewModel.DataCriacao = DateTime.Now;
                estruturaViewModel.DateAlteracao = DateTime.Now;
                estruturaViewModel.idCliente = IDCliente;
                estruturaViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                estruturaViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarEstrutura/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Estrutura = estruturaViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(estruturaViewModel);
        }

        // GET: Estrutura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarEstruturas/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            EstruturaViewModel[] estruturaViewModel = jss.Deserialize<EstruturaViewModel[]>(result);

            if (estruturaViewModel == null)
            {
                return HttpNotFound();
            }
            return View(estruturaViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // POST: Estrutura/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Tipo,idPermissao,idPai,Ativo,Principal,Imagem,ImagemMenu,LinkManual,UrlManual,Ordem,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Status,idCliente")] EstruturaViewModel estruturaViewModel)
        {
            if (ModelState.IsValid)
            {
                estruturaViewModel.DataCriacao = DateTime.Now;
                estruturaViewModel.DateAlteracao = DateTime.Now;
                estruturaViewModel.idCliente = IDCliente;
                estruturaViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                estruturaViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarEstrutura/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Estrutura = estruturaViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }
            return View(estruturaViewModel);
        }

        // GET: Estrutura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstruturaViewModel estruturaViewModel = db.EstruturaViewModels.Find(id);
            if (estruturaViewModel == null)
            {
                return HttpNotFound();
            }
            return View(estruturaViewModel);
        }

        // POST: Estrutura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstruturaViewModel estruturaViewModel = db.EstruturaViewModels.Find(id);
            db.EstruturaViewModels.Remove(estruturaViewModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
