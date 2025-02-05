using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lojinha.Models;
using Lojinha.Domain;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.DataProtection;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly TodoContext _context;

        public PedidoController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Pedido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos.ToListAsync();
        }

        // GET: api/Pedido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        // PUT: api/Pedido/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedido pedido)
        {
            if (id != pedido.IdPedido)
            {
                return BadRequest();
            }

            if (pedido.IdClient == 0)
            {
                return BadRequest("O ID do Cliente não pode ser zero.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == pedido.IdClient);
            if (usuario == null)
            {
                return BadRequest("Cliente não encontrado.");
            }

            if (decimal.IsNegative(pedido.ValorTotal))
            {
                return BadRequest("Valor não pode ser negativo.");
            }
                
            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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

        // POST: api/Pedido
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
           
            if (pedido.IdClient == 0)
            {
                return BadRequest("O ID do Cliente não pode ser zero.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == pedido.IdClient); 
            if (usuario == null)
            {
                return BadRequest("Cliente não encontrado.");
            }

            if (decimal.IsNegative(pedido.ValorTotal))
            {
                return BadRequest("Valor não pode ser negativo.");
            }

            pedido.DataPedido = DateTime.Now;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
        }

        // DELETE: api/Pedido/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
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

            return NoContent();
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }
    }
}
