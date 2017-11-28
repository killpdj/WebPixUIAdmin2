using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers
{
    public class EstiloController : Controller
    {

        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: Page
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarEstilo/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            EstiloViewModel[] estilo = jss.Deserialize<EstiloViewModel[]>(result);

            //var estiloFiltrado = estilo.Where(x => x.idCliente == IDCliente).ToList();


            return View(estilo);
        }

        // GET: estilo/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarEstilo/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            EstiloViewModel[] estiloViewModel = jss.Deserialize<EstiloViewModel[]>(result);

            if (estiloViewModel == null)
            {
                return HttpNotFound();
            }
           var estiloFiltrado = estiloViewModel.Where(x => x.idCliente == IDCliente).FirstOrDefault();

            return View(estiloFiltrado);
        }

        // GET: estilo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: estilo/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Conteudo,Nome,Descricao,Ativo,Status")] EstiloViewModel EstiloViewMode)
        {
            if (ModelState.IsValid)
            {
                EstiloViewMode.DataCriacao = Convert.ToDateTime("01/08/1993");
                EstiloViewMode.DateAlteracao = Convert.ToDateTime("01/08/1993");
                EstiloViewMode.idCliente = IDCliente;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarEstilo/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { estilo = EstiloViewMode };
                    var data = jss.Serialize(Envio);

                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(EstiloViewMode);
        }

        // GET: estilo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarEstilo/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            EstiloViewModel[] estiloViewModel = jss.Deserialize<EstiloViewModel[]>(result);

            if (estiloViewModel == null)
            {
                return HttpNotFound();
            }
            var estiloFiltrado = estiloViewModel.Where(x => x.idCliente == IDCliente).FirstOrDefault();

            return View(estiloFiltrado);
        }

        // POST: estilo/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Conteudo,Nome,Descricao,Ativo,Status")] EstiloViewModel EstiloViewMode)
        {
            EstiloViewMode.UsuarioEdicao = 1;
            EstiloViewMode.idCliente = IDCliente;
            if (ModelState.IsValid)
            {
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarEstilo/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { estilo = EstiloViewMode };
                    var data = jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }
            return View(EstiloViewMode);
        }

        // GET: estilo/Delete/5
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
    }
}
