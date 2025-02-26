using Lojinha.Domain;

namespace Lojinha.Aplicacao.Interfaces
{
    public interface IItemsPedidosService
    {
        public List<ItemsPedidos> ListItemsPedidos();
        public Task<ItemsPedidos> GetItemPedido(int id);
        public Task<bool> UpdateItemPedido(ItemsPedidos itemsPedidos);
        public Task<bool> SaveItemPedido(ItemsPedidos itemsPedidos);
        public Task<bool> DeleteItemPedido(int id);
    }
}
