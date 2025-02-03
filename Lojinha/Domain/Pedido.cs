using System.ComponentModel.DataAnnotations;

namespace Lojinha.Domain
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public int IdClient { get; set; }
        public string? DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
