using Microsoft.AspNetCore.Mvc;
using Lojinha.Models;
using Lojinha.Aplicacao.Interfaces;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _produtosService;

        public ProdutosController(IProdutosService produtosService)
        {
            _produtosService = produtosService;
        }

        // GET: api/Produtos
        [HttpGet]
        public ActionResult<IEnumerable<ProdutosModel>> GetProdutos()
        {
            return _produtosService.ListProdutos().Select(a => a.ToModel()).ToList();
        }

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutosModel>> GetProdutos(int id)
        {
            var produtos = await _produtosService.GetProduto(id);

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

            try
            {
                await _produtosService.UpdateProduto(produtos.ToDomain());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/Produtos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProdutosModel>> PostProdutos(ProdutosModel produtos)
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

            await _produtosService.SaveProduto(produtos.ToDomain());

            return CreatedAtAction("GetProdutos", new { id = produtos.IdProduto }, produtos);
        }

        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdutos(int id)
        {
            try
            {
                await _produtosService.DeleteProduto(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
