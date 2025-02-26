using System.ComponentModel.DataAnnotations;

namespace Lojinha.Domain
{
    public class Fornecedores
    {
        public int IdFornecedor { get; set; }
        public string? NomeFornecedor { get; set; }
        public string? CNPJ { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
    }
}
