using apiAutenticacao.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace apiAutenticacao.Services
{
	//Classe responsável por gerar o token JWT
	//JWT -> JSON Web Token
	public class TokenService
	{

		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration) {

			_configuration = configuration;
		
		}

		public string GerarToken(Usuario usuario) {


			//Recuperamos as informações de configuração do JWT a partir do arquivo appsettings.json	
			string chaveSecreta = _configuration["Jwt:Key"]!;
			string issuer = _configuration["Jwt:Issuer"]!;
			string audience = _configuration["Jwt:Audience"]!;
			int expiracaoHoras = int.Parse(_configuration["Jwt:ExpireHours"]!);

			//Convertemos a chave secreta para bytes, pois o algoritmo de assinatura do token espera uma chave em formato de bytes
			var chaveBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));

			//Definimos as credenciais de assinatura do token, utilizando a chave secreta e o algoritmo de assinatura HMAC SHA256
			var credenciais = new SigningCredentials(chaveBytes, SecurityAlgorithms.HmacSha256);

			//Definimos as claims do token, que são as informações que queremos incluir no token. As claims são pares de chave-valor que representam informações sobre o usuário autenticado. No exemplo, estamos incluindo o email, nome e id do usuário, além de um Jti (JWT ID) que é um identificador único para o token.
			var claims = new[]
			{
				new Claim(ClaimTypes.Email, usuario.Email),
				new Claim(ClaimTypes.Name, usuario.Nome),
				new Claim("id", usuario.Id.ToString()),
				//Id unico do token
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
				
			};

			//Montamos o token com todas as informações necessárias, como o emissor, audiência, claims, data de expiração e credenciais de assinatura


			JwtSecurityToken token = new JwtSecurityToken(
				
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddHours(expiracaoHoras),
				signingCredentials: credenciais
				);


			 string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return tokenString;

		}



	}
}
