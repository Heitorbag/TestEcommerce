using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class MovimentacaoEstoqueModel
    {
        public int IdMovimentacao { get; set; }

        [Required(ErrorMessage = "O Id do Produto é obrigatório.")]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O tipo de movimentação é obrigatório.")]
        [RegularExpression(@"^(ENTRADA|SAIDA)$", ErrorMessage = "O valor deve ser 'ENTRADA' ou 'SAIDA'.")]
        public string? TipoMovimentacao { get; set; }

        [Required(ErrorMessage = "Informar a quantidade é obrigatório.")]
        [Range(0, 999999.99, ErrorMessage = "A quantidade de compra não pode ser negativa.")]
        public decimal Quantidade { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataMovimentacao { get; set; }
        public int IdFornecedor { get; set; }
    }
}
