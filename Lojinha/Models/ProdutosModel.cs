using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class ProdutosModel
    {
        public int IdProduto { get; set; }
        public string? Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal Estoque { get; set; }
    }
}
