using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace meliApi.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ItemsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "APP_USR-86626730255656-060520-6bd078564e177e6cd9ca147d056a6d9d-233127985");
        }

        [HttpGet("me")]
        public async Task<IActionResult> ObtenerListaDeItemsPorUsuario()
        {
            try
            {
                string endpoint = $"https://api.mercadolibre.com/users/me";

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }



        [HttpGet("UnItem")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItem()
        {
            try
            {
                string endpoint = $"https://api.mercadolibre.com/items/MLA1423103713";

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }

        [HttpGet("variosItems")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems()
        {
            try
            {
                string endpoint = $"https://api.mercadolibre.com/items?ids=MLA1423103713,MLA879778758,MLA879778265,MLA879777318";
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }



    }
}