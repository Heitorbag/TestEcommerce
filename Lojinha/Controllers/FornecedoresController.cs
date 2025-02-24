using Microsoft.AspNetCore.Mvc;
using Lojinha.Models;
using Lojinha.Aplicacao;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {

        private readonly IFornecedoresService _fornecedoresService;

        public FornecedoresController(IFornecedoresService fornecedoresService)
        {
            _fornecedoresService = fornecedoresService;
        }

        // GET: api/Fornecedores
        [HttpGet]
        public ActionResult<IEnumerable<FornecedoresModel>> GetFornecedores()
        {
            return _fornecedoresService.ListFornecedores().Select(a => a.ToModel()).ToList();      
        }

        // GET: api/Fornecedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedoresModel>> GetFornecedores(int id)
        {
            var fornecedores = await _fornecedoresService.GetFornecedores(id);

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

            try
            {
                await _fornecedoresService.UpdateFornecedores(fornecedores.ToDomain());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/Fornecedores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FornecedoresModel>> PostFornecedores(FornecedoresModel fornecedores)
        {
            await _fornecedoresService.SaveFornecedores(fornecedores.ToDomain());

            return CreatedAtAction("GetFornecedores", new { id = fornecedores.IdFornecedor }, fornecedores);
        }

        // DELETE: api/Fornecedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedores(int id)
        {
            try
            {
                await _fornecedoresService.DeleteFornecedores(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
