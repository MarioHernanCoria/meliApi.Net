using System.Text.Json;

namespace RefreshToken
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Simular una solicitud que requiere el token
            await SimulateRequest();

            Console.WriteLine("Prueba completada. Presiona cualquier tecla para salir.");
            Console.ReadKey();
        }

        static async Task SimulateRequest()
        {
            // Reemplaza 'api/token/ObtenerToken' con la URL correcta de tu aplicación
            string tokenEndpoint = "https://admin.publicar.shop/meli/api/token/obtenertoken?code=TG-66632f62548c2300011fd270-233127985";

            // Hacer una solicitud al endpoint de obtención de token
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(tokenEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<TokenData>(responseBody);

                Console.WriteLine("Token obtenido con éxito:");
                Console.WriteLine($"Access Token: {tokenData.access_token}");
                Console.WriteLine($"Tipo de Token: {tokenData.token_type}");
                Console.WriteLine($"Expira en (segundos): {tokenData.expires_in}");

                // Simular el paso del tiempo para que el token esté a punto de expirar
                await Task.Delay(30000); // Espera 30 segundos (puedes ajustar el tiempo)

                // Hacer otra solicitud al mismo endpoint para verificar si el token se actualiza automáticamente
                var refreshedResponse = await httpClient.GetAsync(tokenEndpoint);
                if (refreshedResponse.IsSuccessStatusCode)
                {
                    var refreshedBody = await refreshedResponse.Content.ReadAsStringAsync();
                    var refreshedToken = JsonSerializer.Deserialize<TokenData>(refreshedBody);

                    Console.WriteLine("\nToken actualizado automáticamente:");
                    Console.WriteLine($"Nuevo Access Token: {refreshedToken.access_token}");
                    Console.WriteLine($"Nuevo Tipo de Token: {refreshedToken.token_type}");
                    Console.WriteLine($"Expira en (segundos): {refreshedToken.expires_in}");
                }
                else
                {
                    Console.WriteLine("Error al actualizar el token automáticamente.");
                }
            }
            else
            {
                Console.WriteLine("Error al obtener el token.");
            }
        }
    }

    // Clase para deserializar el token
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
