using Lojinha.Domain;

namespace Lojinha.Aplicacao.Interfaces
{
    public interface IEstoqueService
    {
        public List<Estoque> ListEstoque();
        public Task<Estoque> GetEstoque(int id);
        public Task<bool> UpdateEstoque(Estoque estoque);
        public Task<bool> SaveEstoque(Estoque estoque);
        public Task<bool> DeleteEstoque(int id);
    }
}
