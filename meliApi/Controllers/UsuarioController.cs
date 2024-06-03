using meliApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using meliApi.Data;
using Microsoft.AspNetCore.Authorization;

namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioServicio _usuarioServicio;
        private readonly MySqlDbContext _db;
        private readonly HttpClient _httpClient;
        public UsuarioController(UsuarioServicio usuarioServicio, MySqlDbContext db, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _usuarioServicio = usuarioServicio;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuario()
        {
            var usuarioId = _db.Token.Select(t => t.UsuarioId).FirstOrDefault();

            try
            {
                var usuario = await _usuarioServicio.ObtenerUsuario(usuarioId);

                if (usuario == null)
                {
                    return NotFound();
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("UsuarioAutorizado")]
        public async Task<IActionResult> GetUsuarioAutorizado()
        {
            var usuarioId = _db.Token.Select(t => t.UsuarioId).FirstOrDefault();

            try
            {
                var usuario = await _usuarioServicio.AppAutorizadaUsuario(usuarioId);

                if (usuario == null)
                {
                    return NotFound();
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("vendedor")]
        public async Task<IActionResult> ObtenerDatosPublicosDelVendedor()
        {
            try
            {
                // Endpoint para obtener datos públicos del vendedor
                string endpoint = $"https://api.mercadolibre.com/sites/MLA/search?seller_id=233127985";

                // Realizar la solicitud GET al endpoint
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer y devolver la respuesta como una cadena JSON
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
                }
                else
                {
                    // Si la solicitud no fue exitosa, devolver un error
                    return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Manejar errores de solicitud HTTP
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }
    }
}
