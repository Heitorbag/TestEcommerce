using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class ItemsPedidos
    {
        [Key]
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
    }
}
