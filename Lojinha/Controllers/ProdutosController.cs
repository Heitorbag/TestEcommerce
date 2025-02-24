using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lojinha.Models;
using Lojinha.Domain;
using System.Net.Http;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly TodoContext _context;

        public ProdutosController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public ActionResult<IEnumerable<ProdutosModel>> GetProdutos()
        {
            return _context.Produtos.ToListAsync().GetAwaiter().GetResult().Select(a => a.ToModel()).ToList();
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutosModel>> GetProdutos(int id)
        {
            var produtos = await _context.Produtos.FindAsync(id);

            if (produtos == null)
            {
                return NotFound();
            }

            return produtos.ToModel();
        }

        // PUT: api/Produtos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdutos(int id, ProdutosModel produtos)
        {
            if (id != produtos.IdProduto)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(produtos.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            };

            if (!produtos.Nome.All(c => char.IsLetter(c) || c == ' '))
            {
                return BadRequest("O nome deve conter apenas letras.");
            };

            if (decimal.IsNegative(produtos.Estoque))
            {
                return BadRequest("Estoque não pode ser negativo!.");
            }

            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == produtos.IdProduto);
            if (estoque == null || estoque.IdProduto == 0)
            {
                return BadRequest("Produto não encontrado.");
            }

            var itemProdutoAntigo = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(i => i.IdProduto == id);
            if (itemProdutoAntigo == null)
            {
                return BadRequest("Produto antigo não encontrado.");
            }

            estoque.Quantidade = produtos.Estoque;
            estoque.Nome = produtos.Nome;

            _context.Estoque.Update(estoque);
            _context.Entry(produtos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutosExists(id))
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

        // POST: api/Produtos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Produtos>> PostProdutos(ProdutosModel produtos)
        {

            if (string.IsNullOrWhiteSpace(produtos.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            };

            if (!produtos.Nome.All(c => char.IsLetter(c) || c == ' '))
            {
                return BadRequest("O nome deve conter apenas letras.");
            };

            if (produtos.Valor == 0) {
                return BadRequest("O valor não pode ser zero.");
            }

            if (decimal.IsNegative(produtos.Estoque))
            {
                return BadRequest("Estoque não pode ser negativo!.");
            }

            _context.Produtos.Add(produtos.ToDomain());
            await _context.SaveChangesAsync();

            Estoque estoque = new Estoque
            {
                Nome = produtos.Nome,
                IdProduto = produtos.IdProduto,
                Quantidade = produtos.Estoque,
            };

            _context.Estoque.Add(estoque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdutos", new { id = produtos.IdProduto }, produtos);
        }

        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdutos(int id)
        {
            var produtos = await _context.Produtos.FindAsync(id);
            if (produtos == null)
            {
                return NotFound();
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
            if (estoque == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            _context.Estoque.Remove(estoque);
            _context.Produtos.Remove(produtos);
            await _context.SaveChangesAsync();

            return NotFound();
        }

        private bool ProdutosExists(int id)
        {
            return _context.Produtos.Any(e => e.IdProduto == id);
        }
    }
}
