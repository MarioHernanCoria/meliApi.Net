using meliApi.Data;
using meliApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenServicios _tokenServicios;
        private readonly MySqlDbContext _db;
        private readonly ILogger<TokenController> _logger;

        public TokenController(TokenServicios tokenServicios, MySqlDbContext db, ILogger<TokenController> logger)
        {
            _tokenServicios = tokenServicios;
            _db = db;
            _logger = logger;
        }

        [ProducesResponseType(typeof(TokenData), StatusCodes.Status200OK)]
        [HttpGet("ObtenerToken")]
        public async Task<IActionResult> Token(string code)
        {
            try
            {
                var tokenData = await _tokenServicios.ObtenerToken(code);

                // Renovar el token si es necesario
                await RenewTokenIfNeeded(tokenData);

                return Ok(tokenData);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error de comunicación con el servidor de Mercado Libre: {ex.Message}");
                return BadRequest($"Error de comunicación con el servidor de Mercado Libre: {ex.Message}");
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error al procesar la respuesta JSON: {ex.Message}");
                return BadRequest($"Error al procesar la respuesta JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error interno del servidor: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }


        private async Task RenewTokenIfNeeded(TokenData token)
        {
            var expirationDate = DateTime.UtcNow.AddSeconds(token.expires_in);
            var timeUntilExpiration = expirationDate - DateTime.UtcNow;

            // Renovar el token si está a punto de expirar (por ejemplo, 5 minutos antes)
            if (timeUntilExpiration <= TimeSpan.FromMinutes(5))
            {
                // Utilizar el refresh token para obtener un nuevo token
                var newToken = await _tokenServicios.RenewToken(token.refresh_token);

                // Actualizar el token en la base de datos
                token.access_token = newToken.access_token;
                token.expires_in = newToken.expires_in;
                token.token_type = newToken.token_type;

                // Actualizar la fecha de expiración
                token.ExpirationDate = DateTime.UtcNow.AddSeconds(newToken.expires_in);

                await _db.SaveChangesAsync();
            }
        }
    }
}