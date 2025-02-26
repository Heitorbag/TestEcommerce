using Lojinha.Domain;

namespace Lojinha.Aplicacao.Interfaces
{
    public interface IProdutosService
    {
        public List<Produtos> ListProdutos();
        public Task<Produtos> GetProduto(int id);
        public Task<bool> UpdateProduto(Produtos produtos);
        public Task<bool> SaveProduto(Produtos produtos);
        public Task<bool> DeleteProduto(int id);
    }
}
