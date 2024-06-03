using System.Net.Http.Headers;
using System.Text.Json;
using meliApi.Data;
using meliApi.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace meliApi.Servicios
{
    public class TokenServicios
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MySqlDbContext _db;
        private readonly IConfiguration _configuration;

        public TokenServicios(IHttpClientFactory httpClientFactory, MySqlDbContext db, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _db = db;
            _configuration = configuration;
        }

        public async Task<TokenData> ObtenerToken(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("El código de autorización no fue proporcionado.");
            }

            var clientId = _configuration["MercadoLibre:ClientId"];
            var clientSecret = _configuration["MercadoLibre:ClientSecret"];
            var redirectUri = _configuration["MercadoLibre:RedirectUri"];
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
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsync("https://api.mercadolibre.com/oauth/token", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<TokenData>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (tokenData == null)
                {
                    throw new HttpRequestException("Error al deserializar la respuesta del token.");
                }

                // Guardar el token en la base de datos
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

                return tokenData;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener el token de Mercado Libre. Estado de respuesta: {response.StatusCode}, Detalles: {errorResponse}");
            }
        }


        public async Task<TokenData> RenewToken(string refreshToken)
        {
            var clientId = _configuration["MercadoLibre:ClientId"];
            var clientSecret = _configuration["MercadoLibre:ClientSecret"];
            var grantType = "refresh_token";

            var requestData = new
            {
                grant_type = grantType,
                client_id = clientId,
                client_secret = clientSecret,
                refresh_token = refreshToken
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestData));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsync("https://api.mercadolibre.com/oauth/token", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<TokenData>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (tokenData == null)
                {
                    throw new HttpRequestException("Error al deserializar la respuesta del token.");
                }

                return tokenData;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al renovar el token de Mercado Libre. Estado de respuesta: {response.StatusCode}, Detalles: {errorResponse}");
            }
        }
    }
    public class TokenData
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public int user_id { get; set; }
        public string refresh_token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
