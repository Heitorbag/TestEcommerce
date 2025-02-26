using Lojinha.Aplicacao.Interfaces;
using Lojinha.Domain;
using Lojinha.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Aplicacao.Services
{
    public class FornecedoresService : IFornecedoresService
    {
        private LojinhaContext _context;
        public FornecedoresService(LojinhaContext context)
        {
            _context = context;
        }

        public List<Fornecedores> ListFornecedores()
        {
            return _context.Fornecedores.ToListAsync().GetAwaiter().GetResult().ToList();
        }

        public async Task<Fornecedores> GetFornecedores(int id)
        {
            var fornecedores = await _context.Fornecedores.FindAsync(id);
            if (fornecedores is null)
            {
                throw new Exception("Fornecedor não encontrado.");
            }

            return fornecedores;
        }

        public async Task<bool> UpdateFornecedores(Fornecedores fornecedores)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(fornecedores);
            if (fornecedor is null)
            {
                throw new Exception("Fornecedor não encontrado.");
            }

            _context.Entry(fornecedores).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveFornecedores(Fornecedores fornecedores)
        {
            _context.Fornecedores.Add(fornecedores);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteFornecedores(int id)
        {
            var fornecedores = await _context.Fornecedores.FindAsync(id);
            if (fornecedores is null)
            {
                throw new Exception("Fornecedor não encontrado.");
            }

            _context.Fornecedores.Remove(fornecedores);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
