using Microsoft.AspNetCore.Mvc;
using Lojinha.Models;
using Lojinha.Aplicacao;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsPedidosController : ControllerBase
    {
        private readonly IItemsPedidosService _itemsPedidosService;

        public ItemsPedidosController(IItemsPedidosService itemsPedidosService)
        {
            _itemsPedidosService = itemsPedidosService;
        }

        // GET: api/ItemsPedidos
        [HttpGet]
        public ActionResult<IEnumerable<ItemsPedidosModel>> GetItemsPedidos()
        {
            return _itemsPedidosService.ListItemsPedidos().Select(a => a.ToModel()).ToList();        
        }

        // GET: api/ItemsPedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemsPedidosModel>> GetItemsPedidos(int id)
        {
            var itemsPedidos = await _itemsPedidosService.GetItemPedido(id);

            if (itemsPedidos == null)
            {
                return NotFound();
            }

            return itemsPedidos.ToModel();
        }

        // PUT: api/ItemsPedidos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemsPedidos(int id, ItemsPedidosModel itemsPedidos)
        {

            if (id != itemsPedidos.Id)
            {
                return BadRequest("Item pedido não encontrado.");
            }

            if (itemsPedidos.IdPedido == 0 || itemsPedidos.IdProduto == 0 || itemsPedidos.Valor == 0)
            {
                return BadRequest("O valor não pode ser zero.");
            }

            try
            {
                await _itemsPedidosService.UpdateItemPedido(itemsPedidos.ToDomain());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/ItemsPedidos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemsPedidosModel>> PostItemsPedidos(ItemsPedidosModel itemsPedidos)
        {

            if (itemsPedidos.IdPedido == 0 || itemsPedidos.IdProduto == 0 || itemsPedidos.Valor == 0)
            {
                return BadRequest("O valor não pode ser zero.");
            }

            await _itemsPedidosService.SaveItemPedido(itemsPedidos.ToDomain());

            return CreatedAtAction("GetItemsPedidos", new { id = itemsPedidos.Id }, itemsPedidos);
        }

        // DELETE: api/ItemsPedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemsPedidos(int id)
        {
            try
            {
                await _itemsPedidosService.DeleteItemPedido(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
