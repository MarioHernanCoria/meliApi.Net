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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
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

        [HttpPost("catalogo/modelos-recomendados")]
        public async Task<IActionResult> ObtenerModelosRecomendados()
        {
            //Obtener Modelo recomendado según categoría
            //Primero se ejecuta un POST sin BODY.Esto permite traer los valores más usados para el atributo informado.
            //curl - X POST https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/BRAND/top_values

            try
            {
                string endpoint = $"https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/MODEL/top_values";

                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, null);

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

        [HttpPost("catalogo/combinar-atributos")]
        public async Task<IActionResult> CombinarAtributos()
        {
            try
            {
                string endpoint = "https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/TRIM/top_values";

                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, null);

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
