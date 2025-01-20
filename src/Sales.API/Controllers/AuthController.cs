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

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "Usuário e senha são obrigatórios." });
            }

            if (request.Username == "admin" && request.Password == "123456") // Simulação de autenticação
            {
                var token = _jwtService.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Credenciais inválidas." });
        }
    }
}
