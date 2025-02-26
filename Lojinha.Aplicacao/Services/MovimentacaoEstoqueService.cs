using Lojinha.Aplicacao.Interfaces;
using Lojinha.Domain;
using Lojinha.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Aplicacao.Services
{
    public class MovimentacaoEstoqueService : IMovimentacaoEstoqueService
    {
        private LojinhaContext _context;

        public MovimentacaoEstoqueService(LojinhaContext context)
        {
            _context = context;
        }

        public List<MovimentacaoEstoque> ListMovimentacaoEstoque()
        {
            return _context.MovimentacaoEstoque.ToListAsync().GetAwaiter().GetResult().ToList();
        }

        public async Task<MovimentacaoEstoque> GetMovimentacaoEstoque(int id)
        {
            var movimentacaoEstoque = await _context.MovimentacaoEstoque.FindAsync(id);
            if (movimentacaoEstoque is null)
            {
                throw new Exception("Movimentação não encontrada.");
            }

            return movimentacaoEstoque;
        }

        public async Task<bool> UpdateMovimentacaoEstoque(MovimentacaoEstoque movimentacaoEstoque)
        {
            var movimenta = await _context.MovimentacaoEstoque.FindAsync(movimentacaoEstoque);
            if (movimenta is null)
            {
                throw new Exception("Movimentação não encontrada.");
            }

            _context.Entry(movimentacaoEstoque).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveMovimentacaoEstoque(MovimentacaoEstoque movimentacaoEstoque)
        {
            movimentacaoEstoque.DataMovimentacao = DateTime.Now;

            _context.MovimentacaoEstoque.Add(movimentacaoEstoque);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteMovimentacaoEstoque(int id)
        {
            var movimentacaoEstoque = await _context.MovimentacaoEstoque.FindAsync(id);
            if (movimentacaoEstoque is null)
            {
                throw new Exception("Movimentação não encontrada.");
            }

            _context.MovimentacaoEstoque.Remove(movimentacaoEstoque);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
