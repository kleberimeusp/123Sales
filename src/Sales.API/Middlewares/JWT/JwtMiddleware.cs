using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Sales.API.Middlewares.JWT
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey is missing in configuration");
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Session?.GetString("Authorization");
                if (string.IsNullOrEmpty(token))
                {
                    token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Session?.SetString("Authorization", token);
                    }
                }

                if (!string.IsNullOrEmpty(token))
                {
                    AttachUserToContext(context, token);
                }
                else
                {
                    _logger.LogWarning("No token found in session or Authorization header");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving token from session or headers: {ex.Message}");
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,  
                    ValidateAudience = false, 
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken)
                {
                    context.User = principal;
                    context.Items["UserToken"] = token;
                }
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning($"JWT validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error during JWT validation: {ex.Message}");
            }
        }
    }
}
