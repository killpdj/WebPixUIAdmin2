using System.Collections.Generic;

namespace WebPixUIAdmin.Models.Produto
{
    public class ProdutoSkuViewModel : BaseModel
    {
        public int IDProduto { get; set; }
        public string CodSkuExterno { get; set; }
        public int SkuEstoque { get; set; }
        public int SkuPeso { get; set; }
        public List<PropiedadesViewModel> Propiedade { get; set; }
        public bool Ativo { get; set; }
    }
}