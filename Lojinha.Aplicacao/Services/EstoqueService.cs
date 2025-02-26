using Lojinha.Aplicacao.Interfaces;
using Lojinha.Domain;
using Lojinha.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Aplicacao.Services
{
    public class EstoqueService : IEstoqueService
    {
        private LojinhaContext _context;
        public EstoqueService(LojinhaContext context)
        {
            _context = context;
        }

        public List<Estoque> ListEstoque()
        {
            return _context.Estoque.ToListAsync().GetAwaiter().GetResult().ToList();
        }

        public async Task<Estoque> GetEstoque(int id)
        {
            var estoque = await _context.Estoque.FindAsync(id);
            if (estoque is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            return estoque;
        }

        public async Task<bool> UpdateEstoque(Estoque estoque)
        {
            var estoques = await _context.Estoque.FindAsync(estoque);
            if (estoques is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == estoque.IdProduto);
            if (produtos is null || produtos.IdProduto == 0)
            {
                throw new Exception("Produto não encontrado.");
            }

            produtos.Estoque = estoque.Quantidade;
            produtos.Nome = estoque.Nome;

            _context.Produtos.Update(produtos);
            _context.Entry(estoque).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveEstoque(Estoque estoque)
        {
            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.Nome == estoque.Nome);
            if (produtos is null)
            {
                produtos = new Produtos
                {
                    Nome = estoque.Nome,
                    Estoque = estoque.Quantidade,
                    Valor = 0,
                };

                _context.Produtos.Add(produtos);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Produto já existe.");
            }

            estoque.IdProduto = produtos.IdProduto;

            _context.Estoque.Add(estoque);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEstoque(int id)
        {
            var estoque = await _context.Estoque.FindAsync(id);
            if (estoque is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            var itemPedidos = await _context.ItemsPedidos.FirstOrDefaultAsync(i => i.IdProduto == estoque.IdProduto);
            if (itemPedidos != null)
            {
                var pedidos = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == itemPedidos.IdPedido);
                if (pedidos != null)
                {
                    pedidos.ValorTotal = pedidos.ValorTotal - itemPedidos.Valor;
                    _context.Pedidos.Update(pedidos);
                }
                _context.ItemsPedidos.Remove(itemPedidos);
            }

            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == estoque.IdProduto);
            if (produtos is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            _context.Produtos.Remove(produtos);
            _context.Estoque.Remove(estoque);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
