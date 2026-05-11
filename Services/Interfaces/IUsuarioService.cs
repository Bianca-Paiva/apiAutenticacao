using apiAutenticacao.Models;
using apiAutenticacao.Models.DTO;

namespace apiAutenticacao.Services.Interfaces
{
	public interface IUsuarioService
	{
		Task<List<Usuario>> GetAllUsers();
		Task<Usuario?> GetUserById(int id); 
	}
}
