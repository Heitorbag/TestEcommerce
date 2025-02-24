using Microsoft.AspNetCore.Mvc;
using Lojinha.Models;
using Lojinha.Aplicacao;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // GET: api/Pedido
        [HttpGet]
        public ActionResult<IEnumerable<PedidoModel>> GetPedidos()
        {
            return _pedidoService.ListPedidos().Select(a => a.ToModel()).ToList();
        }

        // GET: api/Pedido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoModel>> GetPedido(int id)
        {
            var pedido = await _pedidoService.GetPedido(id);

            return pedido.ToModel();
        }

        // PUT: api/Pedido/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, PedidoModel pedido)
        {
            if (id != pedido.IdPedido)
            {
                return BadRequest();
            }

            if (pedido.IdClient == 0)
            {
                return BadRequest("O ID do Cliente não pode ser zero.");
            }       

            if (decimal.IsNegative(pedido.ValorTotal))
            {
                return BadRequest("Valor não pode ser negativo.");
            }
                       
            try
            {
               await _pedidoService.UpdatePedido(pedido.ToDomain());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/Pedido
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PedidoModel>> PostPedido(PedidoModel pedido)
        {
            if (ModelState.IsValid)
            {
                return BadRequest("Pedido não é valido.");
            }
           
            if (pedido.IdClient == 0)
            {
                return BadRequest("O ID do Cliente não pode ser zero.");
            }  

            if (decimal.IsNegative(pedido.ValorTotal))
            {
                return BadRequest("Valor não pode ser negativo.");
            }

            await _pedidoService.SavePedido(pedido.ToDomain());

            return CreatedAtAction("GetPedido", new { id = pedido.IdPedido }, pedido);
        }

        // DELETE: api/Pedido/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {        
            try
            {
               await _pedidoService.DeletePedido(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
