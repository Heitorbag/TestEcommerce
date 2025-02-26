using Lojinha.Domain;

namespace Lojinha.Aplicacao.Interfaces
{
    public interface IMovimentacaoEstoqueService
    {
        public List<MovimentacaoEstoque> ListMovimentacaoEstoque();
        public Task<MovimentacaoEstoque> GetMovimentacaoEstoque(int id);
        public Task<bool> UpdateMovimentacaoEstoque(MovimentacaoEstoque movimentacaoEstoque);
        public Task<bool> SaveMovimentacaoEstoque(MovimentacaoEstoque movimentacaoEstoque);
        public Task<bool> DeleteMovimentacaoEstoque(int id);
    }
}
