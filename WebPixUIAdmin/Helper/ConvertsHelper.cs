using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebPixUIAdmin.Models;
using WebPixUIAdmin.Models.Principal;

namespace WebPixUIAdmin.Helper
{
    public static class ConvertsHelper
    {
        public static PageModel ConvertToPageModel(PageViewModel page)
        {
            PageModel pageModel = new PageModel
            {
                Conteudo = Encoding.UTF8.GetBytes(page.Conteudo),
                DataCriacao = page.DataCriacao,
                DateAlteracao = page.DateAlteracao,
                Descricao = page.Descricao,
                ID = page.ID,
                idCliente = page.idCliente,
                idMenu = page.idMenu,
                Nome = page.Nome,
                Status = page.Status,
                Titulo = page.Titulo,
                Url = page.Url,
                UsuarioCriacao = page.UsuarioCriacao,
                UsuarioEdicao = page.UsuarioEdicao,
                Ativo = Convert.ToBoolean(page.Ativo)
            };


            return pageModel;
        }
        public static PageViewModel ConvertToPageViewModel(PageModel page)
        {
            PageViewModel pageModel = new PageViewModel
            {
                Conteudo = Encoding.UTF8.GetString(page.Conteudo),
                DataCriacao = page.DataCriacao,
                DateAlteracao = page.DateAlteracao,
                Descricao = page.Descricao,
                ID = page.ID,
                idCliente = page.idCliente,
                idMenu = page.idMenu,
                Nome = page.Nome,
                Status = page.Status,
                Titulo = page.Titulo,
                Url = page.Url,
                UsuarioCriacao = page.UsuarioCriacao,
                UsuarioEdicao = page.UsuarioEdicao,
                Ativo = page.Ativo
            };


            return pageModel;
        }

    }
}