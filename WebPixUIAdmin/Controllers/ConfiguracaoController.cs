using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebPixUIAdmin.Models;
using WebPixUIAdmin.PixCore;

namespace WebPixUIAdmin.Controllers
{
    public class ConfiguracaoController : Controller
    {
        private int IDCliente = PixCore.PixCoreValues.IDCliente;

        // GET: Page
        public ActionResult Index()
        {
            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarconfiguracoes/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ConfiguracaoViewModel[] Configuracao = jss.Deserialize<ConfiguracaoViewModel[]>(result);

            //var ConfiguracaoFiltrado = Configuracao.Where(x => x.idCliente == IDCliente).ToList();

            var teste = 1;
            return View(Configuracao);
        }

        // GET: Configuracao/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarconfiguracoes/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ConfiguracaoViewModel[] ConfiguracaoViewModel = jss.Deserialize<ConfiguracaoViewModel[]>(result);

            if (ConfiguracaoViewModel == null)
            {
                return HttpNotFound();
            }
            var ConfiguracaoFiltrado = ConfiguracaoViewModel.Where(x => x.idCliente == IDCliente).FirstOrDefault();

            return View(ConfiguracaoFiltrado);
        }

        // GET: Configuracao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Configuracao/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Chave,Valor,Nome,Descricao,Ativo,Status")] ConfiguracaoViewModel ConfiguracaoViewMode)
        {
            if (ModelState.IsValid)
            {
                ConfiguracaoViewMode.DataCriacao = Convert.ToDateTime("01/08/1993");
                ConfiguracaoViewMode.DateAlteracao = Convert.ToDateTime("01/08/1993");
                ConfiguracaoViewMode.idCliente = IDCliente;
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarConfiguracao/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Configuracao = ConfiguracaoViewMode };
                    var data = jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }

            return View(ConfiguracaoViewMode);
        }

        // GET: Configuracao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var url = keyUrl + "Seguranca/Principal/buscarconfiguracoes/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
            var client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            var result = client.DownloadString(string.Format(url));
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            ConfiguracaoViewModel[] ConfiguracaoViewModel = jss.Deserialize<ConfiguracaoViewModel[]>(result);

            if (ConfiguracaoViewModel == null)
            {
                return HttpNotFound();
            }
            var ConfiguracaoFiltrado = ConfiguracaoViewModel.Where(x => x.idCliente == IDCliente).FirstOrDefault();

            return View(ConfiguracaoFiltrado);
        }

        // POST: Configuracao/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Chave,Valor,Nome,Descricao,Ativo,Status")] ConfiguracaoViewModel ConfiguracaoViewMode)
        {
            ConfiguracaoViewMode.UsuarioEdicao = 1;
            ConfiguracaoViewMode.idCliente = IDCliente;
            if (ModelState.IsValid)
            {
                using (var client = new WebClient())
                {
                    var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
                    var url = keyUrl + "Seguranca/Principal/salvarConfiguracao/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    var Envio = new { Configuracao = ConfiguracaoViewMode };
                    var data = jss.Serialize(Envio);
                    var result = client.UploadString(url, "POST", data);
                }
                return RedirectToAction("Index");
            }
            return View(ConfiguracaoViewMode);
        }

        // GET: Configuracao/Delete/5
        public JsonResult Delete(int? id)
        {

            var keyUrl = ConfigurationManager.AppSettings["UrlAPI"].ToString();
            var jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            var Envio = new { Configuracao =  new { idConfiguracao = id }  };
            var data = jss.Serialize(Envio);
            try
            {
                using (var client = new WebClient())
                {
                    var url = keyUrl + "Seguranca/Principal/DeletarConfiguracao/" + IDCliente + "/" + PixCoreValues.UsuarioLogado.IdUsuario;
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
