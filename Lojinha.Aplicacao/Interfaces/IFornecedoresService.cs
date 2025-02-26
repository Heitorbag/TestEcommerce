using Lojinha.Domain;

namespace Lojinha.Aplicacao.Interfaces
{
    public interface IFornecedoresService
    {
        public List<Fornecedores> ListFornecedores();
        public Task<Fornecedores> GetFornecedores(int id);
        public Task<bool> UpdateFornecedores(Fornecedores fornecedores);
        public Task<bool> SaveFornecedores(Fornecedores fornecedores);
        public Task<bool> DeleteFornecedores(int id);
    }
}
