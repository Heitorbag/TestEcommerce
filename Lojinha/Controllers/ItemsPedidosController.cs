using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lojinha.Models;

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
                return BadRequest();
            }

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
            _context.ItemsPedidos.Add(itemsPedidos);
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
                return NotFound();
            }

            _context.ItemsPedidos.Remove(itemsPedidos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemsPedidosExists(int id)
        {
            return _context.ItemsPedidos.Any(e => e.Id == id);
        }
    }
}
