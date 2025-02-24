using System.ComponentModel.DataAnnotations;

namespace Lojinha.Domain
{
    public class MovimentacaoEstoque
    {
        [Key]
        public int IdMovimentacao { get; set; }
        public int IdProduto { get; set; }
        public string? TipoMovimentacao { get; set; }
        public decimal Quantidade { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public int IdFornecedor { get; set; }
    }
}
