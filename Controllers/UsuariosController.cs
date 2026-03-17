using apiAutenticacao.Data;
using apiAutenticacao.Models;
using apiAutenticacao.Models.DTO;
using apiAutenticacao.Models.Response;
using apiAutenticacao.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace apiAutenticacao.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UsuarioService _usuarioService;

        public UsuariosController(AuthService authService, UsuarioService usuarioService) { 
        
            _authService = authService;
			_usuarioService = usuarioService;


		}

        [HttpGet("GetUsers")]
		public async Task<IActionResult> GetAllUsers() {
            List<Usuario> usuarios = await _usuarioService.GetAllUsers();
            return Ok(usuarios);

		}

		[HttpPost("cadastrar")]
       public async Task<IActionResult> CadastrarUsuarioAsync([FromBody] CadastroUsuarioDTO dadosUsuario) {

            if (!ModelState.IsValid) { 

                return BadRequest(ModelState);
            }


            ResponseCadastro response = await _authService.CadastrarUsuarioAsync(dadosUsuario);

            if (response.Erro) {

                return BadRequest(response);
            
            }


            return Ok(response);

        }

       [HttpPost("login")]
       public async Task<IActionResult> Login([FromBody] LoginDTO dadosUsuario) {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             ResponseLogin response = await _authService.Login(dadosUsuario);

            if (response.Erro)
            {
                return BadRequest(response);
            }

            return Ok(response);




        }



     [HttpPut("alterarSenha")]
		public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDTO dadosUsuario)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			ResponseAlterarSenhaDTO response = await _authService.AlterarSenha(dadosUsuario);

			if (response.Erro)
			{
				return BadRequest(response);
			}

			return Ok(response);




		}


	}
}
