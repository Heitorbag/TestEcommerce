using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class ItemsPedidosModel
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
        public decimal Quantidade { get; set; }
    }
}
