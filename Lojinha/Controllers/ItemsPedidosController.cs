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
    public class ItemsPedidosController : ControllerBase
    {
        private readonly TodoContext _context;

        public ItemsPedidosController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/ItemsPedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemsPedidos>>> GetItemsPedidos()
        {
            return await _context.ItemsPedidos.ToListAsync();
        }

        // GET: api/ItemsPedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemsPedidos>> GetItemsPedidos(int id)
        {
            var itemsPedidos = await _context.ItemsPedidos.FindAsync(id);

            if (itemsPedidos == null)
            {
                return NotFound();
            }

            return itemsPedidos;
        }

        // PUT: api/ItemsPedidos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemsPedidos(int id, ItemsPedidos itemsPedidos)
        {

            if (id != itemsPedidos.Id)
            {
                return BadRequest("Item pedido não encontrado.");
            }

            if (itemsPedidos.IdPedido == 0 || itemsPedidos.IdProduto == 0 || itemsPedidos.Valor == 0)
            {
                return BadRequest("O valor não pode ser zero.");
            }

            var itemValorAntigo = await _context.ItemsPedidos.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (itemValorAntigo == null)
            {
                return NotFound("Item pedido não encontrado.");
            }

            var pedidoAntigo = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == itemValorAntigo.IdPedido);
            var pedidos = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == itemsPedidos.IdPedido);
            if (pedidos == null || pedidoAntigo == null)
            {
                return BadRequest("Pedido não encontrado.");
            }

            var produtoAntigo = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == itemValorAntigo.IdProduto);
            var estoqueAntigo = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == itemValorAntigo.IdProduto);
            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == itemsPedidos.IdProduto);
            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == itemsPedidos.IdProduto);
            if (estoque == null || produtos == null || produtoAntigo == null || estoqueAntigo == null)
            {
                return BadRequest("Produto não encontrado.");
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
                return BadRequest("Produto fora de estoque.");
            }

            _context.Pedidos.Update(pedidoAntigo);
            _context.Produtos.Update(produtos);
            _context.Estoque.Update(estoque);
            _context.Pedidos.Update(pedidos);
            _context.Entry(itemsPedidos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemsPedidosExists(id))
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

        // POST: api/ItemsPedidos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemsPedidos>> PostItemsPedidos(ItemsPedidos itemsPedidos)
        {

            if (itemsPedidos.IdPedido == 0 || itemsPedidos.IdProduto == 0 || itemsPedidos.Valor == 0)
            {
                return BadRequest("O valor não pode ser zero.");
            }
         
            var pedidos = await _context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == itemsPedidos.IdPedido);
            if (pedidos == null)
            {
                return BadRequest("Pedido não encontrado.");
            }

            var produtos = await _context.Produtos.FirstOrDefaultAsync(p => p.IdProduto == itemsPedidos.IdProduto );
            if (produtos == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.IdProduto == itemsPedidos.IdProduto);
            if (estoque == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            itemsPedidos.Quantidade = itemsPedidos.Valor / produtos.Valor;
            if (itemsPedidos.Quantidade > estoque.Quantidade)
            {
                return BadRequest("Produto fora de estoque.");
            }

            pedidos.ValorTotal = pedidos.ValorTotal + itemsPedidos.Valor;  
            estoque.Quantidade = estoque.Quantidade - itemsPedidos.Quantidade;
            produtos.Estoque = estoque.Quantidade;

            _context.Estoque.Update(estoque);
            _context.Pedidos.Update(pedidos);
            _context.ItemsPedidos.Add(itemsPedidos);
            _context.Produtos.Update(produtos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemsPedidos", new { id = itemsPedidos.Id }, itemsPedidos);
        }

        // DELETE: api/ItemsPedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemsPedidos(int id)
        {
            var itemsPedidos = await _context.ItemsPedidos.FindAsync(id);
            if (itemsPedidos == null)
            {
                return BadRequest("Item pedido não encontrado.");
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

            return NoContent();
        }

        private bool ItemsPedidosExists(int id)
        {
            return _context.ItemsPedidos.Any(e => e.Id == id);
        }
    }
}
