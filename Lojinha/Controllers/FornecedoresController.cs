using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lojinha.Domain;
using Lojinha.Models;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly TodoContext _context;

        public FornecedoresController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Fornecedores
        [HttpGet]
        public ActionResult<IEnumerable<FornecedoresModel>> GetFornecedores()
        {
            return _context.Fornecedores.ToListAsync().GetAwaiter().GetResult().Select(a => a.ToModel()).ToList();
        }

        // GET: api/Fornecedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedoresModel>> GetFornecedores(int id)
        {
            var fornecedores = await _context.Fornecedores.FindAsync(id);

            if (fornecedores == null)
            {
                return NotFound();
            }

            return fornecedores.ToModel();
        }

        // PUT: api/Fornecedores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedores(int id, FornecedoresModel fornecedores)
        {
            if (id != fornecedores.IdFornecedor)
            {
                return BadRequest();
            }

            _context.Entry(fornecedores).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedoresExists(id))
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

        // POST: api/Fornecedores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Fornecedores>> PostFornecedores(FornecedoresModel fornecedores)
        {
            _context.Fornecedores.Add(fornecedores.ToDomain());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFornecedores", new { id = fornecedores.IdFornecedor }, fornecedores);
        }

        // DELETE: api/Fornecedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedores(int id)
        {
            var fornecedores = await _context.Fornecedores.FindAsync(id);
            if (fornecedores == null)
            {
                return NotFound();
            }

            _context.Fornecedores.Remove(fornecedores);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedoresExists(int id)
        {
            return _context.Fornecedores.Any(e => e.IdFornecedor == id);
        }
    }
}
