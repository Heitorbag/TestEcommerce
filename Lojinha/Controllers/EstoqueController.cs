using Microsoft.AspNetCore.Mvc;
using Lojinha.Models;
using Lojinha.Aplicacao.Interfaces;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {     
        private readonly IEstoqueService _estoqueService;

        public EstoqueController(IEstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        // GET: api/Estoque
        [HttpGet]
        public ActionResult<IEnumerable<EstoqueModel>> GetEstoque()
        {
            return _estoqueService.ListEstoque().Select(a => a.ToModel()).ToList();
        }

        // GET: api/Estoque/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstoqueModel>> GetEstoque(int id)
        {
            var estoque = await _estoqueService.GetEstoque(id);

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

            try
            {
                await _estoqueService.UpdateEstoque(estoque.ToDomain());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/Estoque
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstoqueModel>> PostEstoque(EstoqueModel estoque)
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

            await _estoqueService.SaveEstoque(estoque.ToDomain());

            return CreatedAtAction("GetEstoque", new { id = estoque.Id }, estoque);
        }

        // DELETE: api/Estoque/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstoque(int id)
        {
            try
            {
                await _estoqueService.DeleteEstoque(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
