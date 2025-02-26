using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lojinha.Domain
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdClient { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
