using System.Net.Http.Headers;
using System.Text.Json;
using meliApi.Data;
using meliApi.Entidades;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json.Linq;

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
                    ExpirationDate = DateTime.Now.AddSeconds(tokenData.expires_in).AddMilliseconds(-DateTime.Now.Millisecond)

                };
                token.ExpirationDate.AddMilliseconds(0);
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

                // Guardar el token en la base de datos
                var newtoken = new Token
                {
                    AccessToken = tokenData.access_token,
                    TokenType = tokenData.token_type,
                    ExpiresIn = tokenData.expires_in,
                    Scope = tokenData.scope,
                    UsuarioId = tokenData.user_id,
                    RefreshToken = tokenData.refresh_token,
                    ExpirationDate = DateTime.Now.AddSeconds(tokenData.expires_in).AddMilliseconds(-DateTime.Now.Millisecond)
                };

                _db.Token.Add(newtoken);
                await _db.SaveChangesAsync();

                return tokenData;
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al renovar el token de Mercado Libre. Estado de respuesta: {response.StatusCode}, Detalles: {errorResponse}");
            }
        }

        public async Task<TokenData> ObtenerUltimoToken()
        {

            var ultimoToken = await _db.Token.OrderByDescending(t => t.ExpirationDate).FirstOrDefaultAsync();
            if (ultimoToken == null)
            {
                return null;
            }

            DateTime expirationDate = DateTime.Parse(ultimoToken.ExpirationDate.ToString());

            
            return new TokenData
            {
                access_token = ultimoToken.AccessToken,
                token_type = ultimoToken.TokenType,
                expires_in = (int)(DateTime.Now - expirationDate).TotalSeconds,
                scope = ultimoToken.Scope,
                user_id = ultimoToken.UsuarioId,
                refresh_token = ultimoToken.RefreshToken,
                ExpirationDate = expirationDate
            };
        }


        public async Task RefreshToken(TokenData token)
        {

            var date = _db.Token.Max(s => s.ExpirationDate);
            DateTime expirationDate = Convert.ToDateTime(date);
            var timeUntilExpiration = expirationDate - DateTime.Now;

            // Renovar el token si está a punto de expirar (por ejemplo, 5 minutos antes)
            if (timeUntilExpiration > TimeSpan.FromHours(5))
            {
                // Utilizar el refresh token para obtener un nuevo token
                var newToken = await RenewToken(token.refresh_token);


                // Actualizar el token en la base de datos
                token.access_token = newToken.access_token;
                token.expires_in = newToken.expires_in;
                token.token_type = newToken.token_type;

                // Actualizar la fecha de expiración
                token.ExpirationDate = DateTime.Now.AddSeconds(newToken.expires_in);

                await _db.SaveChangesAsync();
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
