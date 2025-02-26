using Lojinha.Aplicacao.Interfaces;
using Lojinha.Domain;
using Lojinha.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Lojinha.Aplicacao.Services
{
    public class UsuarioService : IUsuarioService
    {
        private LojinhaContext _context;
        public UsuarioService(LojinhaContext context)
        {
            _context = context;
        }

        public List<Usuario> ListUsuario()
        {
            return _context.Usuarios.ToListAsync().GetAwaiter().GetResult().ToList();
        }

        public async Task<Usuario> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario is null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            return usuario;
        }

        public async Task<bool> UpdateUsuario(Usuario usuario)
        {
            int id = usuario.Id;

            var usuarios = await _context.Pedidos.FindAsync(id);
            if (usuarios is null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            var verificarEmail = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (verificarEmail is null)
            {
                throw new Exception("E-mail não encontrado.");
            }

            if (verificarEmail.Email != usuario.Email)
            {
                bool emailExistente = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
                if (emailExistente)
                {
                    throw new Exception("Já existe um usuário cadastrado com este e-mail.");
                }
            }

            _context.Entry(usuario).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveUsuario(Usuario usuario)
        {
            bool emailExistente = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailExistente)
            {
                throw new Exception("Já existe um usuário cadastrado com este e-mail.");
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario is null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            var pedidos = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdClient == usuario.Id);
            if (pedidos != null)
            {
                var itemPedidos = await _context.ItemsPedidos.FirstOrDefaultAsync(i => i.IdPedido == pedidos.IdPedido);
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
                _context.Pedidos.Remove(pedidos);
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
