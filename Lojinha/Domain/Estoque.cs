using System.ComponentModel.DataAnnotations;

namespace Lojinha.Domain
{
    public class Estoque
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O Id do Produto é obrigatório.")]
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "Informar a quantidade em estoque é obrigatório.")]
        [Range(0, 999999.99, ErrorMessage = "A quantidade em estoque não pode ser negativa.")]
        public decimal Quantidade { get; set; }
    }
}
