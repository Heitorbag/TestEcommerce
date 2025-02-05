using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class EstoqueModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int IdProduto { get; set; }
        public decimal Quantidade { get; set; }
        public string? DataEntrada { get; set; }
        public string? DataSaida { get; set; }
    }
}
