using System.ComponentModel.DataAnnotations;

namespace Lojinha.Domain
{
    public class Estoque
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int IdProduto { get; set; }
        public decimal Quantidade { get; set; }
    }
}
