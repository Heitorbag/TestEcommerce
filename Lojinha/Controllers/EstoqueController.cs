using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lojinha.Models;
using Lojinha.Domain;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly TodoContext _context;

        public EstoqueController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Estoque
        [HttpGet]
        public ActionResult<IEnumerable<EstoqueModel>> GetEstoque()
        {
            return _context.Estoque.ToListAsync().GetAwaiter().GetResult().Select(a => a.ToModel()).ToList();
        }

        // GET: api/Estoque/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstoqueModel>> GetEstoque(int id)
        {
            var estoque = await _context.Estoque.FindAsync(id);

            if (estoque == null)
            {
                return NotFound();
            }

            return estoque.ToModel();
        }

        // PUT: api/Estoque/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstoque(int id, EstoqueModel estoque)
        {
            if (id != estoque.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(estoque.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            };

            if (!estoque.Nome.All(char.IsLetter))
            {
                return BadRequest("O nome deve conter apenas letras.");
            };

            if (decimal.IsNegative(estoque.Quantidade))
            {
                return BadRequest("Quantidade não pode ser negativa!.");
            }

            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == estoque.IdProduto);
            if (produtos == null || produtos.IdProduto == 0)
            {
                return BadRequest("Produto não encontrado.");
            }

            var itemEstoqueAntigo = await _context.Estoque.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (itemEstoqueAntigo == null)
            {
                return BadRequest("Produto antigo não encontrado.");
            }

            produtos.Estoque = estoque.Quantidade;
            produtos.Nome = estoque.Nome;   

            _context.Produtos.Update(produtos);
            _context.Entry(estoque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstoqueExists(id))
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

        // POST: api/Estoque
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estoque>> PostEstoque(EstoqueModel estoque)
        {
            if (string.IsNullOrWhiteSpace(estoque.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            };

            if (!estoque.Nome.All(c => char.IsLetter(c) || c == ' '))
            {
                return BadRequest("O nome deve conter apenas letras.");
            };

            if (decimal.IsNegative(estoque.Quantidade))
            {
                return BadRequest("Quantidade não pode ser negativa!.");
            }

            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.Nome == estoque.Nome);
            if (produtos == null)
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
                return BadRequest("Produto já existe!");
            }

            estoque.IdProduto = produtos.IdProduto;

            _context.Estoque.Add(estoque.ToDomain());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstoque", new { id = estoque.Id }, estoque);
        }

        // DELETE: api/Estoque/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstoque(int id)
        {
            var estoque = await _context.Estoque.FindAsync(id);
            if (estoque == null)
            {
                return NotFound();
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
            if (produtos == null)
            {
                return BadRequest("Produto não encontrado");
            }

            _context.Produtos.Remove(produtos);
            _context.Estoque.Remove(estoque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstoqueExists(int id)
        {
            return _context.Estoque.Any(e => e.Id == id);
        }
    }
}
