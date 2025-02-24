using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class ProdutosModel
    {
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Informar o valor é obrigatório.")]
        [Range(0, 999999.99, ErrorMessage = "O valor não pode ser negativo.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Informar a quantidade em estoque é obrigatório.")]
        [Range(0, 999999.99, ErrorMessage = "A quantidade em estoque não pode ser negativa.")]
        public decimal Estoque { get; set; }    
    }
}
