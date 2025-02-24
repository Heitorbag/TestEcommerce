using Microsoft.AspNetCore.Mvc;
using Lojinha.Models;
using Lojinha.Aplicacao;
using System.ComponentModel.DataAnnotations;

namespace Lojinha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/Usuario
        [HttpGet]
        public ActionResult<IEnumerable<UsuarioModel>> GetUsuarios()
        {
            return _usuarioService.ListUsuario().Select(a => a.ToModel()).ToList();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioModel>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuario(id);
    
            return usuario.ToModel();
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioModel usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            };

            if (!usuario.Nome.All(c => char.IsLetter(c) || c == ' '))
            {
                return BadRequest("O nome deve conter apenas letras.");
            };

            if (string.IsNullOrWhiteSpace(usuario.Email) || !new EmailAddressAttribute().IsValid(usuario.Email))
            {
                return BadRequest("O E-mail fornecido é inválido.");
            };

            if (string.IsNullOrWhiteSpace(usuario.Endereco))
            {
                return BadRequest("O endereço é obrigatório.");
            };

            try
            {
                await _usuarioService.UpdateUsuario(usuario.ToDomain());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioModel>> PostUsuario(UsuarioModel usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            };

            if (!usuario.Nome.All(c => char.IsLetter(c) || c == ' '))
            {
                return BadRequest("O nome deve conter apenas letras.");
            };

            if (string.IsNullOrWhiteSpace(usuario.Email) || !new EmailAddressAttribute().IsValid(usuario.Email))
            {
                return BadRequest("O E-mail fornecido é inválido.");
            };

            if (string.IsNullOrWhiteSpace(usuario.Endereco))
            {
                return BadRequest("O endereço é obrigatório.");
            };

            await _usuarioService.SaveUsuario(usuario.ToDomain());

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                await _usuarioService.DeleteUsuario(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
