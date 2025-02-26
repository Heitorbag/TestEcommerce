using Lojinha.Domain;

namespace Lojinha.Aplicacao.Interfaces
{
    public interface IPedidoService
    {
        public List<Pedido> ListPedidos();
        public Task<Pedido> GetPedido(int id);
        public Task<bool> UpdatePedido(Pedido pedido);
        public Task<bool> SavePedido(Pedido pedido);
        public Task<bool> DeletePedido(int id);
    }
}
