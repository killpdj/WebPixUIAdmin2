using System;

namespace WebPixUIAdmin.Models.Produto
{
    public class PrecoViewModel : BaseModel
    {
        public int IDProduto { get; set; }
        public int PrecoReal { get; set; }
        public int PrecoPromocional { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public bool Ativo { get; set; }
    }
}