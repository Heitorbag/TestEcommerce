using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lojinha.Models;
using Lojinha.Domain;
using System.ComponentModel.DataAnnotations;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly TodoContext _context;

        public UsuarioController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public ActionResult<IEnumerable<UsuarioModel>> GetUsuarios()
        {
            return _context.Usuarios.ToListAsync().GetAwaiter().GetResult().Select(a => a.ToModel()).ToList();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioModel>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario.ToModel();
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioModel usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            };

            if (!usuario.Nome.All(c => char.IsLetter(c) || c == ' '))
            {
                return BadRequest("O nome deve conter apenas letras.");
            };

            if (string.IsNullOrWhiteSpace(usuario.Email) || !new EmailAddressAttribute().IsValid(usuario.Email))
            {
                return BadRequest("O E-mail fornecido é inválido.");
            };

            if (string.IsNullOrWhiteSpace(usuario.Endereco))
            {
                return BadRequest("O endereço é obrigatório.");
            };

            var verificarEmail = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (verificarEmail == null) 
            {
                return BadRequest("E-mail não encontrado.");
            }     

            if (verificarEmail.Email != usuario.Email)
            {
                bool emailExistente = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
                if (emailExistente)
                {
                    return BadRequest("Já existe um usuário cadastrado com este e-mail.");
                }
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(UsuarioModel usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            };

            if (!usuario.Nome.All(c => char.IsLetter(c) || c == ' '))
            {
                return BadRequest("O nome deve conter apenas letras.");
            };

            if (string.IsNullOrWhiteSpace(usuario.Email) || !new EmailAddressAttribute().IsValid(usuario.Email))
            {
                return BadRequest("O E-mail fornecido é inválido.");
            };

            if (string.IsNullOrWhiteSpace(usuario.Endereco))
            {
                return BadRequest("O endereço é obrigatório.");
            };

            bool emailExistente = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailExistente)
            {
                return BadRequest("Já existe um usuário cadastrado com este e-mail.");
            }

            _context.Usuarios.Add(usuario.ToDomain());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
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

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
