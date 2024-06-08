using meliApi.Data.Repositories.Interface;
using meliApi.Entidades;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static meliApi.Controllers.MeliController;

namespace meliApi.Servicios.Items
{
    public class ItemService : IItemService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenServicios _tokenServicios;
        private readonly IProductosCollection _productoCollection;

        public ItemService(HttpClient httpClient, TokenServicios tokenServicios, IProductosCollection productosCollection)
        {
            _productoCollection = productosCollection;
            _httpClient = httpClient;
            _tokenServicios = tokenServicios;
        }


        public async Task<List<string>> GetItemsByUserIdAsync(int userId)
        {
            await SetAuthorizationHeaderAsync();

            string endpoint = $"https://api.mercadolibre.com/users/{userId}/items/search";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                ApiResponse responseObj = JsonConvert.DeserializeObject<ApiResponse>(json);
                return responseObj.results;
            }
            else
            {
                throw new HttpRequestException($"La solicitud falló con el código de estado: {response.StatusCode}");
            }
        }

        public async Task<string> GetItemDescriptionAsync(string itemId)
        {
            await SetAuthorizationHeaderAsync();
            string endpoint = $"https://api.mercadolibre.com/items/{itemId}/description";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"La solicitud falló con el código de estado: {response.StatusCode}");
            }
        }

        public async Task<Producto> GetItemSpecificationsAsync(string itemId)
        {
            await SetAuthorizationHeaderAsync();
            string endpoint = $"https://api.mercadolibre.com/items/{itemId}";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Producto producto = JsonConvert.DeserializeObject<Producto>(json);
                await _productoCollection.InsertProducto(producto);
                return producto;
            }
            else
            {
                throw new HttpRequestException($"La solicitud falló con el código de estado: {response.StatusCode}");
            }
        }

        public async Task<string> GetItemSpecificationsWithAttributesAsync(string itemIds, string attributes)
        {
            await SetAuthorizationHeaderAsync();
            string endpoint = $"https://api.mercadolibre.com/items?ids={itemIds}&attributes={attributes}";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"La solicitud falló con el código de estado: {response.StatusCode}");
            }
        }

        public async Task<string> GetMultipleItemSpecificationsAsync(string itemIds)
        {
            await SetAuthorizationHeaderAsync();
            string endpoint = $"https://api.mercadolibre.com/items?ids={itemIds}";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"La solicitud falló con el código de estado: {response.StatusCode}");
            }
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            try
            {
                var tokenData = await _tokenServicios.ObtenerUltimoToken();

                if (tokenData.expires_in > 21600)
                {
                    await _tokenServicios.RenewToken(tokenData.refresh_token);
                    tokenData = await _tokenServicios.ObtenerUltimoToken();
                }

                if (tokenData != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData.access_token);
                }
                else
                {
                    throw new InvalidOperationException("No se pudo obtener un token válido.");
                }
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Error al establecer el encabezado de autorización.", ex);
            }
        }
    }

}
