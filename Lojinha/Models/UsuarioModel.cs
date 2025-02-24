using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(255, ErrorMessage = "O endereço deve ter no máximo 255 caracteres.")]
        public string? Endereco { get; set; }    
    }
}
