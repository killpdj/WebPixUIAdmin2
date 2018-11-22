using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPixUIAdmin.Models.Principal
{
    public class PageModel : BaseModel
    {
        public string Titulo { get; set; }
        public byte[] Conteudo { get; set; }
        public string Url { get; set; }
        public int idMenu { get; set; }
    }
}