using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }
        public int IdClient { get; set; }
        public string? DataPedido { get; set; }
        public int ValorTotal { get; set; }
    }
}
