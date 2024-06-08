using meliApi.Data;
using meliApi.Entidades;
using meliApi.Servicios;
using meliApi.Servicios.Items;
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
        private readonly ItemService _itemService;

        public TokenController(TokenServicios tokenServicios, MySqlDbContext db, ILogger<TokenController> logger, ItemService itemService)
        {
            _itemService = itemService;
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
                if (tokenData == null)
                {
                    return BadRequest("No se encontró ningún token válido.");
                }

                await _tokenServicios.RefreshToken(tokenData);

                var usuarioId = tokenData.user_id;
                List<string> itemsMeli = await _itemService.GetItemsByUserIdAsync(usuarioId);

                List<Producto> itemSpecificationsList = new List<Producto>();

                if (itemsMeli != null && itemsMeli.Count > 0)
                {
                    // Recorrer cada itemId en itemsMeli y obtener sus especificaciones
                    foreach (var itemId in itemsMeli)
                    {
                        var itemSpecifications = await _itemService.GetItemSpecificationsAsync(itemId);
                        itemSpecificationsList.Add(itemSpecifications);
                    }
                    return Ok(tokenData);
                }
                else
                {
                    return Ok(new { TokenData = tokenData, Message = "No se encontraron items para el usuario." });
                }
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


        [HttpGet("ObtenerRefreshToken")]
        public async Task<IActionResult> RefreshToken(string tokenRefresh)
        {
            try
            {
                var tokenData = await _tokenServicios.RenewToken(tokenRefresh);

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




        
    }
}