using Lojinha.Aplicacao.Interfaces;
using Lojinha.Domain;
using Lojinha.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Aplicacao.Services
{
    public class ProdutosService : IProdutosService
    {
        private LojinhaContext _context;
        public ProdutosService(LojinhaContext context)
        {
            _context = context;
        }

        public List<Produtos> ListProdutos()
        {
            return _context.Produtos.ToListAsync().GetAwaiter().GetResult().ToList();
        }

        public async Task<Produtos> GetProduto(int id)
        {
            var produtos = await _context.Produtos.FindAsync(id);

            if (produtos is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            return produtos;
        }

        public async Task<bool> UpdateProduto(Produtos produtos)
        {
            var produto = await _context.Produtos.FindAsync(produtos);
            if (produto is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == produtos.IdProduto);
            if (estoque is null || estoque.IdProduto is 0)
            {
                throw new Exception("Produto não encontrado.");
            }

            estoque.Quantidade = produtos.Estoque;
            estoque.Nome = produtos.Nome;

            _context.Estoque.Update(estoque);
            _context.Entry(produtos).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveProduto(Produtos produtos)
        {
            _context.Produtos.Add(produtos);
            await _context.SaveChangesAsync();

            Estoque estoque = new Estoque
            {
                Nome = produtos.Nome,
                IdProduto = produtos.IdProduto,
                Quantidade = produtos.Estoque,
            };

            _context.Estoque.Add(estoque);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduto(int id)
        {
            var produtos = await _context.Produtos.FindAsync(id);
            if (produtos is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            var itemPedidos = await _context.ItemsPedidos.FirstOrDefaultAsync(i => i.IdProduto == produtos.IdProduto);
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

            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == produtos.IdProduto);
            if (estoque is null)
            {
                throw new Exception("Produto não encontrado.");
            }

            _context.Estoque.Remove(estoque);
            _context.Produtos.Remove(produtos);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
