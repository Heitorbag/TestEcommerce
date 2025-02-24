using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class PedidoModel
    {
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "O Id do cliente é obrigatório.")]
        public int IdClient { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataPedido { get; set; }

        [Range(0, 999999.99, ErrorMessage = "O valor total não pode ser negativo.")]
        public decimal ValorTotal { get; set; }

        public string? Teste { get; set; }
    }
}
