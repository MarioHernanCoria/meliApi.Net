using meliApi.Data;
using meliApi.Entidades;
using meliApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeliController : ControllerBase
    {
        private readonly MeliTokenServicio _meliTokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _db;
        private string requestBody;

        public MeliController(MeliTokenServicio meliTokenService, IHttpClientFactory httpClientFactory, AppDbContext db, HttpClient httpClient)
        {
            _meliTokenService = meliTokenService;
            _httpClientFactory = httpClientFactory;
            _db = db;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "APP_USR-86626730255656-051204-54755567add5497d0d03959f559ccab2-233127985");
        }

        [HttpGet("ObetenerToken")]
        public async Task<IActionResult> AuthorizationCallback(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest("El código de autorización no fue proporcionado.");
                }

                var clientId = "86626730255656";
                var clientSecret = "Yv14soVZWD7xCWwlguzO42YaAqVy984m";
                var redirectUri = "https://publicar.shop";
                var grantType = "authorization_code";

                var requestData = new
                {
                    grant_type = grantType,
                    client_id = clientId,
                    client_secret = clientSecret,
                    code = code,
                    redirect_uri = redirectUri
                };

                var requestContent = new StringContent(JsonSerializer.Serialize(requestData));

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.PostAsync("https://api.mercadolibre.com/oauth/token", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var tokenData = JsonSerializer.Deserialize<TokenData>(responseBody);

                    var token = new Token
                    {
                        AccessToken = tokenData.access_token,
                        TokenType = tokenData.token_type,
                        ExpiresIn = tokenData.expires_in,
                        Scope = tokenData.scope,
                        UsuarioId = tokenData.user_id,
                        RefreshToken = tokenData.refresh_token,
                        ExpirationDate = DateTime.UtcNow.AddSeconds(tokenData.expires_in)
                    };

                    // Guardar el token en la base de datos
                    _db.Token.Add(token);
                    await _db.SaveChangesAsync();

                    // Si el token ha expirado, renovarlo
                    if (TokenExpirado(token))
                    {
                        var (nuevoAccessToken, nuevoRefreshToken) = await RenovarToken(token.RefreshToken);

                        // Actualizar los tokens en la base de datos
                        token.AccessToken = nuevoAccessToken;
                        token.RefreshToken = nuevoRefreshToken;
                        token.ExpirationDate = DateTime.UtcNow.AddSeconds(tokenData.expires_in);

                        await _db.SaveChangesAsync();
                    }

                    IniciarProcesoPeriodico();

                    return Ok(responseBody);
                }
                else
                {
                    // Manejar el caso de error de la solicitud
                    return BadRequest($"Error al obtener el token de Mercado Libre. Estado de respuesta: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error de comunicación con el servidor de Mercado Libre: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return BadRequest($"Error al procesar la respuesta JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }


        private bool TokenExpirado(Token token)
        {
            return DateTime.UtcNow > token.ExpirationDate;
        }

        private async Task<(string NuevoAccessToken, string NuevoRefreshToken)> RenovarToken(string refreshToken)
        {
            try
            {
                var clientId = "86626730255656";
                var clientSecret = "Yv14soVZWD7xCWwlguzO42YaAqVy984m";
                var grantType = "refresh_token";

                var requestData = new
                {
                    grant_type = grantType,
                    client_id = clientId,
                    client_secret = clientSecret,
                    refresh_token = refreshToken
                };

                var requestContent = new StringContent(JsonSerializer.Serialize(requestData));

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.PostAsync("https://api.mercadolibre.com/oauth/token", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var tokenData = JsonSerializer.Deserialize<TokenData>(responseBody);

                    return (tokenData.access_token, tokenData.refresh_token);
                }
                else
                {
                    // Manejar el caso de error de la solicitud
                    throw new Exception($"Error al renovar el token. Estado de respuesta: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores
                throw new Exception($"Error al renovar el token: {ex.Message}");
            }
        }

        // Método para verificar y actualizar periódicamente los tokens
        private async Task VerificarYActualizarTokensPeriodicamente()
        {
            // Obtener todos los tokens de la base de datos
            var tokens = _db.Token.ToList();

            foreach (var token in tokens)
            {
                // Verificar si el token ha expirado
                if (TokenExpirado(token))
                {
                    // Renovar el token
                    var (nuevoAccessToken, nuevoRefreshToken) = await RenovarToken(token.RefreshToken);

                    // Actualizar los tokens en la base de datos
                    token.AccessToken = nuevoAccessToken;
                    token.RefreshToken = nuevoRefreshToken;
                    token.ExpirationDate = DateTime.UtcNow.AddSeconds(token.ExpiresIn);

                    await _db.SaveChangesAsync();
                }
            }
        }

        // Método para ejecutar el proceso periódicamente
        private async Task EjecutarProcesoPeriodico()
        {
            while (true)
            {
                // Verificar y actualizar los tokens
                await VerificarYActualizarTokensPeriodicamente();

                // Esperar un cierto tiempo antes de volver a verificar los tokens
                await Task.Delay(TimeSpan.FromMinutes(30)); // Ejemplo: verifica cada 30 minutos
            }
        }

        // Método para iniciar el proceso periódico en la aplicación
        private void IniciarProcesoPeriodico()
        {
            Task.Run(EjecutarProcesoPeriodico);
        }




        [HttpGet("items/especificaciones")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems([FromQuery] string ids, [FromQuery] string attributes)
        {
                //Especificaciones de varios ítems y con atributos específicos:
                //https://api.mercadolibre.com/items?ids=$ITEM_ID1,$ITEM_ID2&attributes=$ATTRIBUTE1,$ATTRIBUTE2,$ATTRIBUTE3
                //https://api.mercadolibre.com/items?ids=MLA1423103713&attributes=title,price,currency_id,condition
            try
            {
                // Endpoint para obtener especificaciones de varios ítems con atributos específicos
                string endpoint = $"https://api.mercadolibre.com/items?ids={ids}&attributes={attributes}";

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

        [HttpGet("items/{itemId}/descripcion")]
        public async Task<IActionResult> ObtenerDescripcionDeItem(string itemId)
        {

            //Obtener la descripción del ítem:
            //curl - X GET - H 'Authorization: Bearer $ACCESS_TOKEN' https://api.mercadolibre.com/items/$ITEM_ID/description
            //https://api.mercadolibre.com/items/MLA1423103713/description
            try
            {
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

        [HttpGet("categorias/{categoryId}/atributos")]
        public async Task<IActionResult> ObtenerAtributosDeCategoria(string categoryId)
        {
            //Obtener atributos de una categoría
            //Son todos los atributos que posee una categoría específica.
            //https://api.mercadolibre.com/categories/MLA1744/attributes

            try
            {
                // Endpoint para obtener los atributos de una categoría específica
                string endpoint = $"https://api.mercadolibre.com/categories/{categoryId}/attributes";

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

        [HttpPost("catalogo/modelos-recomendados")]
        public async Task<IActionResult> ObtenerModelosRecomendados()
        {
            //Obtener Modelo recomendado según categoría
            //Primero se ejecuta un POST sin BODY.Esto permite traer los valores más usados para el atributo informado.
            //curl - X POST https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/BRAND/top_values
           
            try
            {
                // Endpoint para obtener los modelos recomendados según la categoría
                string endpoint = $"https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/MODEL/top_values";

                // Realizar la solicitud POST al endpoint (sin body en este caso)
                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, null);

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

        [HttpPost("catalogo/combinar-atributos")]
        public async Task<IActionResult> CombinarAtributos()
        {
            try
            {
                // Endpoint para combinar atributos
                string endpoint = "https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/TRIM/top_values";

                // Realizar la solicitud POST al endpoint (sin requestBody en este caso)
                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, null);

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

