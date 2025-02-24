using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class FornecedoresModel
    {
        public int IdFornecedor { get; set; }

        [Required(ErrorMessage = "O nome do fornecedor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do fornecedor deve ter no máximo 100 caracteres.")]
        public string? NomeFornecedor { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [RegularExpression(@"\d{14}", ErrorMessage = "O CNPJ deve conter 14 dígitos numéricos.")]
        public string? CNPJ { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(255, ErrorMessage = "O endereço deve ter no máximo 255 caracteres.")]
        public string? Endereco { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [RegularExpression(@"^\(\d{2}\) \d{4,5}-\d{4}$", ErrorMessage = "O telefone deve estar no formato (XX) XXXX-XXXX ou (XX) XXXXX-XXXX.")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string? Email { get; set; }
    }
}
