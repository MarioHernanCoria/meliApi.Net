using meliApi.Data;
using meliApi.Data.Repositories.Interface;
using meliApi.Entidades;
using meliApi.Servicios;
using meliApi.Servicios.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IItemService _itemService;

        public MeliController(IHttpClientFactory httpClientFactory, MySqlDbContext db, HttpClient httpClient, TokenServicios tokenServicios, IProductosCollection productosCollection, IItemService itemService)
        {
            _httpClientFactory = httpClientFactory;
            _db = db;
            _httpClient = httpClient;
            _tokenServicios = tokenServicios;
            _productoCollection = productosCollection;
            _itemService = itemService;
        }



        [HttpGet("items/ItemsPorUsuario")]
        public async Task<IActionResult> ItemsPorUsuario(int userId)
        {
            try
            {
                List<string> items = await _itemService.GetItemsByUserIdAsync(userId);
                return Ok(items);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerBrand()
        {
            try
            {

                string endpoint = "https://admin.publicar.shop/meli/api/callback";

                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, null);

                if (response.IsSuccessStatusCode)
                {
                    return Ok();
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


        [HttpGet("items/EspecificacionesPorItem")]
        public async Task<IActionResult> ObtenerEspecificacionesPorItems(string itemId)
        {
            try
            {
                var item = await _itemService.GetItemSpecificationsAsync(itemId);
                return Ok(item);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }


        [HttpGet("items/descripcion")]
        public async Task<IActionResult> ObtenerDescripcionDeItem(string itemId)
        {
            try
            {
                var description = await _itemService.GetItemDescriptionAsync(itemId);
                return Ok(description);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }


        [HttpGet("items/especificaciones")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems([FromQuery] string itemIds)
        {
            try
            {
                var specifications = await _itemService.GetMultipleItemSpecificationsAsync(itemIds);
                return Ok(specifications);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }


        [HttpGet("items/especificacionesYAtributos")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems([FromQuery] string MLA1423103713, [FromQuery] string atributos)
        {
            try
            {
                var specifications = await _itemService.GetItemSpecificationsWithAttributesAsync(MLA1423103713, atributos);
                return Ok(specifications);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error al realizar la solicitud HTTP: {ex.Message}");
            }
        }


        public class ApiResponse
        {
            public string seller_id { get; set; }
            public List<string> results { get; set; }
            public Paging paging { get; set; }
            // Define otras propiedades según sea necesario
        }

        public class Paging
        {
            public int limit { get; set; }
            public int offset { get; set; }
            public int total { get; set; }
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


