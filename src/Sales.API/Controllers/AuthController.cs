using Microsoft.AspNetCore.Mvc;
using Sales.Application.DTOs;
using Sales.Domain.Interfaces;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IJwtService jwtService, ILogger<AuthController> logger)
        {
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!IsValidLoginRequest(request))
            {
                LogWarning("Invalid login request received.");
                return BadRequest(new { Message = "Usuário e senha são obrigatórios." });
            }

            if (AuthenticateUser(request))
            {
                var token = _jwtService.GenerateToken(request.Username);
                LogInfo("Usuário autenticado com sucesso.");
                return Ok(new { Token = token, Message = "Login realizado com sucesso." });
            }

            LogWarning("Tentativa de login com credenciais inválidas.");
            return Unauthorized(new { Message = "Credenciais inválidas." });
        }

        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            var token = ExtractTokenFromHeader();

            if (string.IsNullOrEmpty(token))
            {
                LogWarning("Token não encontrado.");
                return Unauthorized(new { Message = "Token não encontrado." });
            }

            if (_jwtService.ValidateToken(token))
            {
                LogInfo("Token válido.");
                return Ok(new { Message = "Token válido." });
            }

            LogWarning("Token inválido ou expirado.");
            return Unauthorized(new { Message = "Token inválido ou expirado." });
        }

        // PRIVATE METHODS
        private bool IsValidLoginRequest(LoginRequest request)
        {
            return request != null && !string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password);
        }

        private bool AuthenticateUser(LoginRequest request)
        {
            // Simulação de autenticação
            return request.Username == "admin" && request.Password == "123456";
        }

        private string ExtractTokenFromHeader()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            return authHeader?.StartsWith("Bearer ") == true ? authHeader.Split(" ").Last() : null;
        }

        private void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        private void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
