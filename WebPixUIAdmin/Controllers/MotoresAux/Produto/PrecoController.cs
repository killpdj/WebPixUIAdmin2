using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models.Produto;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers
{
    public class PrecoController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;
        // GET: Preco
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarpreco/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            PrecoViewModel[] Produto = jss.Deserialize<PrecoViewModel[]>(result);

            var PrecoFiltrado = Produto.Where(x => x.idCliente == IDCliente).ToList();

            return View(PrecoFiltrado);
        }

        // GET: Preco/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarpreco/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoViewModel[] precoViewModel = jss.Deserialize<ProdutoViewModel[]>(result);

            if (precoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(precoViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // GET: Preco/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Preco/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDProduto,PrecoReal,PrecoPromocional,DataInicio,DataFinal,Ativo,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Status,idCliente")] PrecoViewModel precoViewModel)
        {
            if (ModelState.IsValid)
            {
                precoViewModel.DataCriacao = DateTime.Now;
                precoViewModel.DateAlteracao = DateTime.Now;
                precoViewModel.idCliente = IDCliente;
                precoViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                precoViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Produto/salvarpreco/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Preco = precoViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(precoViewModel);
        }

        // GET: Preco/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarpreco/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoViewModel[] precoViewModel = jss.Deserialize<ProdutoViewModel[]>(result);

            if (precoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(precoViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // POST: Preco/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDProduto,PrecoReal,PrecoPromocional,DataInicio,DataFinal,Ativo,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Status,idCliente")] PrecoViewModel precoViewModel)
        {
            if (ModelState.IsValid)
            {
                precoViewModel.DataCriacao = DateTime.Now;
                precoViewModel.DateAlteracao = DateTime.Now;
                precoViewModel.idCliente = IDCliente;
                precoViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                precoViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Produto/salvarpreco/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Preco = precoViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }
            return View(precoViewModel);
        }

        // GET: Preco/Delete/5
        public ActionResult Delete(int? id)
        {
           
            return View();
        }

        // POST: Preco/Delete/5
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
