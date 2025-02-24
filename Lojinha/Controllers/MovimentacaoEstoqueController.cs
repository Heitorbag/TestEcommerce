using Microsoft.AspNetCore.Mvc;
using Lojinha.Models;
using Lojinha.Aplicacao;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoEstoqueController : ControllerBase
    {
        private readonly IMovimentacaoEstoqueService _movimentacaoEstoqueService;

        public MovimentacaoEstoqueController(IMovimentacaoEstoqueService movimentacaoEstoqueService)
        {
            _movimentacaoEstoqueService = movimentacaoEstoqueService;
        }

        // GET: api/MovimentacaoEstoque
        [HttpGet]
        public ActionResult<IEnumerable<MovimentacaoEstoqueModel>> GetMovimentacaoEstoque()
        {
            return _movimentacaoEstoqueService.ListMovimentacaoEstoque().Select(a => a.ToModel()).ToList();
        }

        // GET: api/MovimentacaoEstoque/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentacaoEstoqueModel>> GetMovimentacaoEstoque(int id)
        {
            var movimentacaoEstoque = await _movimentacaoEstoqueService.GetMovimentacaoEstoque(id);

            return movimentacaoEstoque.ToModel();
        }

        // PUT: api/MovimentacaoEstoque/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimentacaoEstoque(int id, MovimentacaoEstoqueModel movimentacaoEstoque)
        {
            if (id != movimentacaoEstoque.IdMovimentacao)
            {
                return BadRequest();
            }

            try
            {
                await _movimentacaoEstoqueService.UpdateMovimentacaoEstoque(movimentacaoEstoque.ToDomain());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/MovimentacaoEstoque
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovimentacaoEstoqueModel>> PostMovimentacaoEstoque(MovimentacaoEstoqueModel movimentacaoEstoque)
        {

            await _movimentacaoEstoqueService.SaveMovimentacaoEstoque(movimentacaoEstoque.ToDomain());

            return CreatedAtAction("GetMovimentacaoEstoque", new { id = movimentacaoEstoque.IdMovimentacao }, movimentacaoEstoque);
        }

        // DELETE: api/MovimentacaoEstoque/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimentacaoEstoque(int id)
        {
            try
            {
                await _movimentacaoEstoqueService.DeleteMovimentacaoEstoque(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
