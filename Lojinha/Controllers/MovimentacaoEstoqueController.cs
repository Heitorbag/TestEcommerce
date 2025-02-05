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
    public class MovimentacaoEstoqueController : ControllerBase
    {
        private readonly TodoContext _context;

        public MovimentacaoEstoqueController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/MovimentacaoEstoque
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimentacaoEstoque>>> GetMovimentacaoEstoque()
        {
            return await _context.MovimentacaoEstoque.ToListAsync();
        }

        // GET: api/MovimentacaoEstoque/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentacaoEstoque>> GetMovimentacaoEstoque(int id)
        {
            var movimentacaoEstoque = await _context.MovimentacaoEstoque.FindAsync(id);

            if (movimentacaoEstoque == null)
            {
                return NotFound();
            }

            return movimentacaoEstoque;
        }

        // PUT: api/MovimentacaoEstoque/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimentacaoEstoque(int id, MovimentacaoEstoque movimentacaoEstoque)
        {
            if (id != movimentacaoEstoque.IdMovimentacao)
            {
                return BadRequest();
            }

            _context.Entry(movimentacaoEstoque).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimentacaoEstoqueExists(id))
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

        // POST: api/MovimentacaoEstoque
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovimentacaoEstoque>> PostMovimentacaoEstoque(MovimentacaoEstoque movimentacaoEstoque)
        {

            movimentacaoEstoque.DataMovimentacao = DateTime.Now;

            _context.MovimentacaoEstoque.Add(movimentacaoEstoque);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovimentacaoEstoque", new { id = movimentacaoEstoque.IdMovimentacao }, movimentacaoEstoque);
        }

        // DELETE: api/MovimentacaoEstoque/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimentacaoEstoque(int id)
        {
            var movimentacaoEstoque = await _context.MovimentacaoEstoque.FindAsync(id);
            if (movimentacaoEstoque == null)
            {
                return NotFound();
            }

            _context.MovimentacaoEstoque.Remove(movimentacaoEstoque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimentacaoEstoqueExists(int id)
        {
            return _context.MovimentacaoEstoque.Any(e => e.IdMovimentacao == id);
        }
    }
}
