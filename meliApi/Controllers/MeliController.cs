using meliApi.Data;
using meliApi.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeliController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly MySqlDbContext _db;

        public MeliController(IHttpClientFactory httpClientFactory, MySqlDbContext db, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _db = db;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
        }


        [HttpGet("items/especificaciones")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems([FromQuery] string MLA1423103713, [FromQuery] string atributos)
        {
                //Especificaciones de varios ítems y con atributos específicos:
                //https://api.mercadolibre.com/items?ids=$ITEM_ID1,$ITEM_ID2&attributes=$ATTRIBUTE1,$ATTRIBUTE2,$ATTRIBUTE3
                //https://api.mercadolibre.com/items?ids=MLA1423103713&attributes=title,price,currency_id,condition
            try
            {
                // Endpoint para obtener especificaciones de varios ítems con atributos específicos
                string endpoint = $"https://api.mercadolibre.com/items?ids={MLA1423103713}&attributes={atributos}";

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

        [HttpGet("items/descripcion")]
        public async Task<IActionResult> ObtenerDescripcionDeItem(string MLA1423103713)
        {

            //Obtener la descripción del ítem:
            //curl - X GET - H 'Authorization: Bearer $ACCESS_TOKEN' https://api.mercadolibre.com/items/$ITEM_ID/description
            //https://api.mercadolibre.com/items/MLA1423103713/description
            try
            {
                // Endpoint para obtener la descripción de un ítem específico
                string endpoint = $"https://api.mercadolibre.com/items/{MLA1423103713}/description";

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

        [HttpGet("categorias")]
        public async Task<IActionResult> ObtenerTodasLasCategorias()
        {

            //Obtener todas las categorías
            //https://api.mercadolibre.com/sites/MLA/categories
            try
            {
                // Endpoint para obtener todas las categorías
                string endpoint = "https://api.mercadolibre.com/sites/MLA/categories";

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

        [HttpGet("categorias/atributos")]
        public async Task<IActionResult> ObtenerAtributosDeCategoria(string MLA1744)
        {
            //Obtener atributos de una categoría
            //Son todos los atributos que posee una categoría específica.
            //https://api.mercadolibre.com/categories/MLA1744/attributes

            try
            {
                // Endpoint para obtener los atributos de una categoría específica
                string endpoint = $"https://api.mercadolibre.com/categories/{MLA1744}/attributes";

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

