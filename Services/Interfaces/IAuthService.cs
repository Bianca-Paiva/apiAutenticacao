using apiAutenticacao.Models.DTO;
using apiAutenticacao.Models.Response;

namespace apiAutenticacao.Services.Interfaces
{
	public interface IAuthService
	{
		Task<ResponseLogin> Login(LoginDTO dadosUsuario);
		Task<ResponseCadastro> CadastrarUsuarioAsync(CadastroUsuarioDTO dadosUsuarioCadastro);

		Task<ResponseAlterarSenhaDTO> AlterarSenha(AlterarSenhaDTO dadosUsuarioAlterar);
	}
}