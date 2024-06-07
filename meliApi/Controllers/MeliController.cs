using meliApi.Data;
using meliApi.Data.Repositories.Interface;
using meliApi.Entidades;
using meliApi.Servicios;
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
        private readonly TokenServicios _tokenServicios;
        private readonly IProductosCollection _productoCollection;

        public MeliController(IHttpClientFactory httpClientFactory, MySqlDbContext db, HttpClient httpClient, TokenServicios tokenServicios, IProductosCollection productosCollection)
        {
            _httpClientFactory = httpClientFactory;
            _db = db;
            _httpClient = httpClient;
            _tokenServicios = tokenServicios;
            _productoCollection = productosCollection;
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            var tokenData = await _tokenServicios.ObtenerUltimoToken();
            if (tokenData != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData.access_token);
            }
            else
            {
                throw new InvalidOperationException("No se pudo obtener un token válido.");
            }
        }


        [HttpGet("items/ItemsPorUsuario")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems(int userId)
        {
            
            try
            {
                await SetAuthorizationHeaderAsync();

                // Endpoint para obtener especificaciones de varios ítems con atributos específicos
                string endpoint = $"https://api.mercadolibre.com/users/{userId}/items/search";

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


        [HttpGet("items/EspecificacionesPorItem")]
        public async Task<IActionResult> ObtenerEspecificacionesPorItems(string itemId)
        {

            try
            {
                await SetAuthorizationHeaderAsync();


                // Endpoint para obtener especificaciones de varios ítems con atributos específicos
                string endpoint = $"https://api.mercadolibre.com/items/{itemId}";

                // Realizar la solicitud GET al endpoint
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer y devolver la respuesta como una cadena JSON
                    string json = await response.Content.ReadAsStringAsync();
                    Producto producto = Newtonsoft.Json.JsonConvert.DeserializeObject<Producto>(json);

                    await _productoCollection.InsertProducto(producto);

                    return Ok(producto);
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
        public async Task<IActionResult> ObtenerDescripcionDeItem(string itemId)
        {

            try
            {
                await SetAuthorizationHeaderAsync();

                // Endpoint para obtener la descripción de un ítem específico
                string endpoint = $"https://api.mercadolibre.com/items/{itemId}/description";

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


        [HttpGet("items/especificaciones")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems([FromQuery]string itemIds)
        {
            try
            {
                await SetAuthorizationHeaderAsync();

                // Endpoint para obtener especificaciones de varios ítems con atributos específicos
                string endpoint = $"https://api.mercadolibre.com/items?ids={itemIds}";

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


        [HttpGet("items/especificacionesYAtributos")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems([FromQuery] string MLA1423103713, [FromQuery] string atributos)
        {
            //Especificaciones de varios ítems y con atributos específicos:
            //https://api.mercadolibre.com/items?ids=$ITEM_ID1,$ITEM_ID2&attributes=$ATTRIBUTE1,$ATTRIBUTE2,$ATTRIBUTE3
            //https://api.mercadolibre.com/items?ids=MLA1423103713&attributes=title,price,currency_id,condition
            try
            {
                await SetAuthorizationHeaderAsync();

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
    }
}


//[HttpGet("categorias")]
//public async Task<IActionResult> ObtenerTodasLasCategorias()
//{

//    //Obtener todas las categorías
//    //https://api.mercadolibre.com/sites/MLA/categories
//    try
//    {
//        // Endpoint para obtener todas las categorías
//        string endpoint = "https://api.mercadolibre.com/sites/MLA/categories";

//        // Realizar la solicitud GET al endpoint
//        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

//        // Verificar si la solicitud fue exitosa
//        if (response.IsSuccessStatusCode)
//        {
//            // Leer y devolver la respuesta como una cadena JSON
//            string json = await response.Content.ReadAsStringAsync();
//            return Ok(json);
//        }
//        else
//        {
//            // Si la solicitud no fue exitosa, devolver un error
//            return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
//        }
//    }
//    catch (HttpRequestException ex)
//    {
//        // Manejar errores de solicitud HTTP
//        return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
//    }
//}

//[HttpGet("categorias/atributos")]
//public async Task<IActionResult> ObtenerAtributosDeCategoria(string MLA1744)
//{
//    //Obtener atributos de una categoría
//    //Son todos los atributos que posee una categoría específica.
//    //https://api.mercadolibre.com/categories/MLA1744/attributes

//    try
//    {
//        // Endpoint para obtener los atributos de una categoría específica
//        string endpoint = $"https://api.mercadolibre.com/categories/{MLA1744}/attributes";

//        // Realizar la solicitud GET al endpoint
//        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

//        // Verificar si la solicitud fue exitosa
//        if (response.IsSuccessStatusCode)
//        {
//            // Leer y devolver la respuesta como una cadena JSON
//            string json = await response.Content.ReadAsStringAsync();
//            return Ok(json);
//        }
//        else
//        {
//            // Si la solicitud no fue exitosa, devolver un error
//            return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
//        }
//    }
//    catch (HttpRequestException ex)
//    {
//        // Manejar errores de solicitud HTTP
//        return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
//    }
//}


