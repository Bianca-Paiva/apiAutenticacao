namespace apiAutenticacao.Services
{
    // Classe responsável por gerar o toker JWT
    // JWT = JSON Web Token
    public class TokenService
    {
        // Interface que permite acessar o appsettings.json
        private readonly IConfiguration _configuration;

        // ================= CONSTRUTOR =================
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // MÉTODO: GERAR TOKEN JWT
        // Cria e retorna um token com base nas configurações do sistema
        public string GerarToken() 
        {
            // a exclamaçã(!) diz que não vai ser nulo
            // Chave secreta usada para assinar o token (segurança)
            string chaveSecreta = _configuration["Jwt:Key"]!;

            // Quem está emitindo o token (sua API)
            string issuer = _configuration["Jwt:Issuer"]!;

            // Para quem o token é válido (cliente/usuário)
            string audience = _configuration["Jwt:Audience"]!;

            // Tempo de expiração do token
            int expiracaoHoras = int.Parse(_configuration["Jwt:ExpireHours"]!);
        }
    }
}
