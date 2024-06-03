using meliApi.Data;
using meliApi.Entidades;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

namespace meliApi.Servicios
{
    public class UsuarioServicio
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MySqlDbContext _db;

        public UsuarioServicio(IHttpClientFactory httpClientFactory, MySqlDbContext db)
        {
            _db = db;
            _httpClientFactory = httpClientFactory;
            
        }


        public async Task<Usuario> ObtenerUsuario(int id)
        {

            string url = $"https://api.mercadolibre.com/users/{id}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonResponse);

                    return usuario;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    throw new Exception($"Failed to fetch user with ID {id}. Status code: {response.StatusCode}");
                }
            }
        }


        public async Task<List<UsuarioAutorizado>> AppAutorizadaUsuario(int user_id)
        {
            string token = "";
            string url = $"https://api.mercadolibre.com/users/{user_id}/applications";

            using (HttpClient client = new HttpClient())
            {
                // Configurar el encabezado de autorización con el token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<UsuarioAutorizado> usuariosAutorizados = JsonConvert.DeserializeObject<List<UsuarioAutorizado>>(jsonResponse);

                    return usuariosAutorizados;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    throw new Exception($"Failed to fetch user with ID {user_id}. Status code: {response.StatusCode}");
                }
            }
        }



    }
}
