using meliApi.Data;
using meliApi.Entidades;
using System.Text.Json;

namespace meliApi.Servicios
{
    public class MeliService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MySqlDbContext _db;
        public MeliService(IHttpClientFactory httpClientFactory, MySqlDbContext db)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<string> ObtenerToken(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    throw new ArgumentException("El código de autorización no fue proporcionado.");
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

                    return responseBody;
                }
                else
                {
                    throw new HttpRequestException($"Error al obtener el token de Mercado Libre. Estado de respuesta: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Error de comunicación con el servidor de Mercado Libre: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Error al procesar la respuesta JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
