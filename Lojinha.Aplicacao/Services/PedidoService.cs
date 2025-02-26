using Lojinha.Aplicacao.Interfaces;
using Lojinha.Domain;
using Lojinha.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Aplicacao.Services
{
    public class PedidoService : IPedidoService
    {
        private LojinhaContext _context;
        public PedidoService(LojinhaContext context)
        {
            _context = context;
        }

        public List<Pedido> ListPedidos()
        {
            return _context.Pedidos.ToListAsync().GetAwaiter().GetResult().ToList();
        }

        public async Task<Pedido> GetPedido(int id)
        {
            var pedidos = await _context.Pedidos.FindAsync(id);
            if (pedidos is null)
            {
                throw new Exception("Pedido não encontrado.");
            }
            return pedidos;
        }

        public async Task<bool> UpdatePedido(Pedido pedido)
        {
            var pedidos = await _context.Pedidos.FindAsync(pedido);
            if (pedidos is null)
            {
                throw new Exception("Pedido não encontrado.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == pedido.IdClient);
            if (usuario is null)
            {
                throw new Exception("Usuario não encontrado.");
            }

            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SavePedido(Pedido pedido)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == pedido.IdClient);
            if (usuario is null)
            {
                throw new Exception("Usuario não encontrado.");
            }

            pedido.DataPedido = DateTime.Now;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido is null)
            {
                throw new Exception("Pedido não encontrado.");
            }

            var itemPedidos = await _context.ItemsPedidos.FirstOrDefaultAsync(i => i.IdPedido == pedido.IdPedido);

            if (itemPedidos != null)
            {
                var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == itemPedidos.IdProduto);
                if (estoque != null)
                {
                    estoque.Quantidade += itemPedidos.Quantidade;
                    _context.Estoque.Update(estoque);

                    var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == itemPedidos.IdProduto);
                    if (produto != null)
                    {
                        produto.Estoque = estoque.Quantidade;
                        _context.Produtos.Update(produto);
                    }
                }
                _context.ItemsPedidos.Remove(itemPedidos);
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
