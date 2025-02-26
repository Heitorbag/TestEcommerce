using Lojinha.Aplicacao.Interfaces;
using Lojinha.Domain;
using Lojinha.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Aplicacao.Services
{
    public class ItemsPedidosService : IItemsPedidosService
    {
        private LojinhaContext _context;
        public ItemsPedidosService(LojinhaContext context)
        {
            _context = context;
        }

        public List<ItemsPedidos> ListItemsPedidos()
        {
            return _context.ItemsPedidos.ToListAsync().GetAwaiter().GetResult().ToList();
        }

        public async Task<ItemsPedidos> GetItemPedido(int id)
        {
            var itemPedido = await _context.ItemsPedidos.FindAsync(id);
            if (itemPedido is null)
            {
                throw new Exception("Item pedido não encontrado.");
            }

            return itemPedido;
        }

        public async Task<bool> UpdateItemPedido(ItemsPedidos itemsPedidos)
        {
            int id = itemsPedidos.Id;

            var itemPedido = await _context.ItemsPedidos.FindAsync(id);
            if (itemPedido is null)
            {
                throw new Exception("Item pedido não encontrado.");
            }

            var itemValorAntigo = await _context.ItemsPedidos.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (itemValorAntigo is null)
            {
                throw new Exception("Item pedido não encontrado.");
            }

            var pedidoAntigo = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == itemValorAntigo.IdPedido);
            var pedidos = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == itemsPedidos.IdPedido);
            if (pedidos is null || pedidoAntigo is null)
            {
                throw new Exception("Pedido não encontrado.");
            }

            var produtoAntigo = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == itemValorAntigo.IdProduto);
            var estoqueAntigo = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == itemValorAntigo.IdProduto);
            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == itemsPedidos.IdProduto);
            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == itemsPedidos.IdProduto);
            if (estoque is null || produtos is null || produtoAntigo is null || estoqueAntigo is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            if (itemValorAntigo.IdPedido != itemsPedidos.IdPedido)
            {
                pedidoAntigo.ValorTotal = pedidoAntigo.ValorTotal - itemValorAntigo.Valor;
                if (itemValorAntigo.IdPedido != itemsPedidos.IdPedido && itemsPedidos.Valor == itemValorAntigo.Valor)
                {
                    pedidos.ValorTotal = pedidos.ValorTotal + itemsPedidos.Valor;
                }
            }

            if (itemsPedidos.Valor > itemValorAntigo.Valor)
            {
                if (itemValorAntigo.IdPedido != itemsPedidos.IdPedido)
                {
                    pedidos.ValorTotal = pedidos.ValorTotal + itemsPedidos.Valor;
                }
                else if (itemValorAntigo.IdPedido == itemsPedidos.IdPedido)
                {
                    pedidos.ValorTotal = pedidos.ValorTotal + (itemsPedidos.Valor - itemValorAntigo.Valor);
                }
                estoque.Quantidade = estoque.Quantidade - itemsPedidos.Quantidade;
                produtos.Estoque = estoque.Quantidade;
            }
            else if (itemsPedidos.Valor < itemValorAntigo.Valor)
            {
                if (itemValorAntigo.IdPedido != itemsPedidos.IdPedido)
                {
                    pedidos.ValorTotal = pedidos.ValorTotal + itemsPedidos.Valor;
                }
                else if (itemValorAntigo.IdPedido == itemsPedidos.IdPedido)
                {
                    pedidos.ValorTotal = pedidos.ValorTotal - (itemValorAntigo.Valor - itemsPedidos.Valor);
                }
                estoque.Quantidade = estoque.Quantidade + itemsPedidos.Quantidade;
                produtos.Estoque = estoque.Quantidade;
            }

            if (produtoAntigo.IdProduto != itemsPedidos.IdProduto)
            {
                estoqueAntigo.Quantidade = estoqueAntigo.Quantidade + itemValorAntigo.Quantidade;
                produtoAntigo.Estoque = estoqueAntigo.Quantidade;
            }

            itemsPedidos.Quantidade = itemsPedidos.Valor / produtos.Valor;
            if (itemsPedidos.Quantidade > estoque.Quantidade)
            {
                throw new Exception("Produto fora de estoque.");
            }

            _context.Pedidos.Update(pedidoAntigo);
            _context.Produtos.Update(produtos);
            _context.Estoque.Update(estoque);
            _context.Pedidos.Update(pedidos);
            _context.Entry(itemsPedidos).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveItemPedido(ItemsPedidos itemsPedidos)
        {
            var pedidos = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == itemsPedidos.IdPedido);
            if (pedidos is null)
            {
                throw new Exception("Pedido não encontrado.");
            }

            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == itemsPedidos.IdProduto);
            if (produtos is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == itemsPedidos.IdProduto);
            if (estoque is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            itemsPedidos.Quantidade = itemsPedidos.Valor / produtos.Valor;
            if (itemsPedidos.Quantidade > estoque.Quantidade)
            {
                throw new Exception("Produto fora de estoque.");
            }

            pedidos.ValorTotal = pedidos.ValorTotal + itemsPedidos.Valor;
            estoque.Quantidade = estoque.Quantidade - itemsPedidos.Quantidade;
            produtos.Estoque = estoque.Quantidade;

            _context.Estoque.Update(estoque);
            _context.Pedidos.Update(pedidos);
            _context.ItemsPedidos.Add(itemsPedidos);
            _context.Produtos.Update(produtos);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemPedido(int id)
        {
            var itemsPedidos = await _context.ItemsPedidos.FindAsync(id);
            if (itemsPedidos is null)
            {
                throw new Exception("Item pedido não encontrado.");
            }

            var pedidos = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == itemsPedidos.IdPedido);
            if (pedidos != null)
            {
                pedidos.ValorTotal = pedidos.ValorTotal - itemsPedidos.Valor;
                _context.Pedidos.Update(pedidos);
            }

            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == itemsPedidos.IdProduto);
            if (estoque != null)
            {
                estoque.Quantidade = estoque.Quantidade + itemsPedidos.Quantidade;
                _context.Estoque.Update(estoque);

                var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == itemsPedidos.IdProduto);
                if (produtos != null)
                {
                    produtos.Estoque = estoque.Quantidade;
                    _context.Produtos.Update(produtos);
                }
            }

            _context.ItemsPedidos.Remove(itemsPedidos);
            _context.Entry(itemsPedidos).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
