using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models.Produto;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers.MotoresAux
{
    public class ProdutoController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: Produto
        public ActionResult Index()
        {

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarproduto/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoViewModel[] Produto = jss.Deserialize<ProdutoViewModel[]>(result);

            var ProdutoFiltrado = Produto.Where(x => x.idCliente == IDCliente).ToList();

            return View(ProdutoFiltrado);
        }

        // GET: Produto/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarproduto/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoViewModel[] produtoViewModel = jss.Deserialize<ProdutoViewModel[]>(result);

            if (produtoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(produtoViewModel.Where(z => z.ID == id).FirstOrDefault());

        }

        // GET: Produto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CodExterno,Fabrinte,Estoque,Peso,Catalogo,Comprimento,Largura,Altura,EstoqueMinimo,DescricaoLonga,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Ativo,Status,idCliente")] ProdutoViewModel produtoViewModel)
        {
            if (ModelState.IsValid)
            {
                produtoViewModel.DataCriacao = DateTime.Now;
                produtoViewModel.DateAlteracao = DateTime.Now;
                produtoViewModel.idCliente = IDCliente;
                produtoViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                produtoViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                produtoViewModel.Preco = new PrecoViewModel();
                produtoViewModel.Sku = new List<ProdutoSkuViewModel>();
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Produto/salvarproduto/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { produto = produtoViewModel };
                    var data = JsonConvert.SerializeObject(Envio);  // jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(produtoViewModel);
        }

        // GET: Produto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Produto/buscarproduto/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ProdutoViewModel[] produtoViewModel = jss.Deserialize<ProdutoViewModel[]>(result);

            if (produtoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(produtoViewModel.Where(z => z.ID == id).FirstOrDefault());
        }

        // POST: Produto/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CodExterno,Fabrinte,Estoque,Peso,Catalogo,Comprimento,Largura,Altura,EstoqueMinimo,DescricaoLonga,Nome,Descricao,DataCriacao,DateAlteracao,UsuarioCriacao,UsuarioEdicao,Ativo,Status,idCliente")] ProdutoViewModel produtoViewModel)
        {
            if(ModelState.IsValid)
            {
                produtoViewModel.DataCriacao = DateTime.Now;
                produtoViewModel.DateAlteracao = DateTime.Now;
                produtoViewModel.idCliente = IDCliente;
                produtoViewModel.UsuarioCriacao = PixCoreValues.UsuarioLogado.IdUsuario;
                produtoViewModel.UsuarioEdicao = PixCoreValues.UsuarioLogado.IdUsuario;
                produtoViewModel.Preco = new PrecoViewModel();
                produtoViewModel.Sku = new List<ProdutoSkuViewModel>();

                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Produto/salvarproduto/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { produto = produtoViewModel };
                    var data = jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(produtoViewModel);
        }

        // GET: Produto/Delete/5
        public ActionResult Delete(int? id)
        {
            
            return View();
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //ProdutoViewModel produtoViewModel = db.ProdutoViewModels.Find(id);
            //db.ProdutoViewModels.Remove(produtoViewModel);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
              /// db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
