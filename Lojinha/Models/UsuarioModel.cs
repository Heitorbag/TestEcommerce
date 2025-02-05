using System.ComponentModel.DataAnnotations;

namespace Lojinha.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
    }
}
