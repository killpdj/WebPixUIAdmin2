using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models.Produto;
using WebPixUIAdmin.PixCore;
using System.Collections.Generic;

namespace WebPixUIAdmin.Controllers.MotoresAux
{
    public class ProdutoSkuController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: ProdutoSku
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarprodutosku/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoSkuViewModel[] ProdutoSku = jss.Deserialize<ProdutoSkuViewModel[]>(result);

            var ProdutoSkuFiltrado = ProdutoSku.Where(x => x.idCliente == IDCliente).ToList();

            return View(ProdutoSkuFiltrado);
        }

        // GET: ProdutoSku/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarprodutosku/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoSkuViewModel[] produtoskuViewModel = jss.Deserialize<ProdutoSkuViewModel[]>(result);

            if (produtoskuViewModel == null)
            {
                return HttpNotFound();
            }
            return View(produtoskuViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // GET: ProdutoSku/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProdutoSku/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDProduto,CodSkuExterno,SkuEstoque,SkuPeso,Ativo,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Status,idCliente")] ProdutoSkuViewModel produtoSkuViewModel)
        {
            if (ModelState.IsValid)
            {
                produtoSkuViewModel.DataCriacao = DateTime.Now;
                produtoSkuViewModel.DateAlteracao = DateTime.Now;
                produtoSkuViewModel.idCliente = IDCliente;
                produtoSkuViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                produtoSkuViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                produtoSkuViewModel.Propiedade = new List<PropiedadesViewModel>();

                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Produto/salvarprodutosku/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { ProdutoSku = produtoSkuViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(produtoSkuViewModel);
        }

        // GET: ProdutoSku/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarprodutosku/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoSkuViewModel[] produtoskuViewModel = jss.Deserialize<ProdutoSkuViewModel[]>(result);

            if (produtoskuViewModel == null)
            {
                return HttpNotFound();
            }
            return View(produtoskuViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // POST: ProdutoSku/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDProduto,CodSkuExterno,SkuEstoque,SkuPeso,Ativo,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Status,idCliente")] ProdutoSkuViewModel produtoSkuViewModel)
        {
            if (ModelState.IsValid)
            {
                produtoSkuViewModel.DateAlteracao = DateTime.Now;
                produtoSkuViewModel.idCliente = IDCliente;
                produtoSkuViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;

                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Produto/salvarprodutosku/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { ProdutoSku = produtoSkuViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }
            return View(produtoSkuViewModel);
        }

        // GET: ProdutoSku/Delete/5
        public ActionResult Delete(int? id)
        {
            
            return View();
        }

        // POST: ProdutoSku/Delete/5
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
