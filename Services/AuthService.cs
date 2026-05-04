using apiAutenticacao.Data;
using apiAutenticacao.Models;
using apiAutenticacao.Models.DTO;
using apiAutenticacao.Models.Response;
using apiAutenticacao.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;


namespace apiAutenticacao.Services
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _context;


        public AuthService(AppDbContext context) {

            _context = context;
        
        }


        public async Task<ResponseLogin> Login(LoginDTO dadosUsuario)
        {

            Usuario? usuarioEncontrado = await _context.Usuarios.
                FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuario.Email);

            if (usuarioEncontrado != null)
            {
                bool isValidPassword = Verify(dadosUsuario.Senha, usuarioEncontrado.Senha);

                

                if (isValidPassword)
                {
                    return new ResponseLogin { 
                    
                        Erro = false,
                        Message = "Login Realizado com sucesso",
                        Usuario = new Usuario { 
                        
                            Id = usuarioEncontrado.Id,
							Nome = usuarioEncontrado.Nome,
                            Email = usuarioEncontrado.Email
                        
                        }

                    };

                }

                return new ResponseLogin { 
                
                    Erro = true,
                    Message = "Login não realizado. Email ou senha incorretos",
                    
                };

            }

            return new ResponseLogin { 
            
                Erro = true,
                Message = "Usuário não encontrado",
                
            };


        }

        public async Task<ResponseCadastro> CadastrarUsuarioAsync(CadastroUsuarioDTO dadosUsuarioCadastro) {

            Usuario? usuarioExistente = await _context.Usuarios.
               FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuarioCadastro.Email);

            if (usuarioExistente != null)
            {


                return new ResponseCadastro { 
                
                    Erro = true,
                    Message = "Este email já está cadastrado no sistema"
                
                };

                
            }        

			Usuario usuario = new()
            {

                Nome = dadosUsuarioCadastro.Nome,
                Email = dadosUsuarioCadastro.Email,
                Senha = HashPassword(dadosUsuarioCadastro.Senha),


            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new ResponseCadastro { 
            
                Erro = false,
                Message = "Usuário cadastrado com sucesso",
                Usuario = usuario
                

            } ;

        }

        public async Task<ResponseAlterarSenhaDTO> AlterarSenha(AlterarSenhaDTO dadosUsuarioAlterar) { 


			Usuario? userEncontrado = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == dadosUsuarioAlterar.Email);

			
			if (userEncontrado != null) {

				bool isValidPasswords = Verify(dadosUsuarioAlterar.SenhaAtual, userEncontrado.Senha);

                if (!isValidPasswords)
                {
					return new ResponseAlterarSenhaDTO
					{

						Erro = true,
						Message = "Senha ou email inválidos."

					};
				}

                if (dadosUsuarioAlterar.NovaSenha == dadosUsuarioAlterar.ConfirmarNovaSenha)
                {
					userEncontrado.Senha = HashPassword(dadosUsuarioAlterar.NovaSenha);
					_context.Usuarios.Update(userEncontrado);
					await _context.SaveChangesAsync();
					return new ResponseAlterarSenhaDTO  { 
                    
                        Erro = false,
                        Message = "Senha alterada com sucesso"

					};
				}
            }
			return new ResponseAlterarSenhaDTO
			{

				Erro = true,
				Message = "Senha não pôde ser alterada. Confira seus dados e tente novamente."

			};



		}






    }
}
