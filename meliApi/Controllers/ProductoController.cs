
using meliApi.Data.Repositories.Implementacion;
using meliApi.Data.Repositories.Interface;
using meliApi.Entidades;
using meliApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private IProductosCollection db = new ProductoCollection();
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _clientFactory;
        private readonly TokenServicios _tokenServicios;

        public ProductoController(IHttpClientFactory httpClientFactory, TokenServicios tokenServicios)
        {
            _httpClient = httpClientFactory.CreateClient();
            _tokenServicios = tokenServicios;
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


        [HttpPost("brand")]
        public async Task<IActionResult> ObtenerBrand()
        {
            try
            {
                await SetAuthorizationHeaderAsync();

                string endpoint = $"https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/BRAND/top_values";

                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, null);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
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


        [HttpPost("modelos")]
        public async Task<IActionResult> ObtenerModelos([FromBody] ModeloRequest modeloRequest)
        {
            try
            {
                await SetAuthorizationHeaderAsync();

                var requestBody = new
                {
                    known_attributes = new[]
                    {
                        new
                        {
                            id = "BRAND",
                            value_id = modeloRequest.MarcaId
                        }
                    }
                };

                // Convertir el cuerpo de la solicitud a JSON
                var jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);

                // Realizar la solicitud POST a la URL proporcionada
                var response = await _httpClient.PostAsync("https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/MODEL/top_values",
                                                        new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json"));

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer y devolver la respuesta como una cadena JSON
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
                }
                else
                {
                    // Si la solicitud no fue exitosa, devolver un error con el código de estado
                    return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores generales
                return StatusCode(500, $"Error al realizar la solicitud: {ex.Message}");
            }
        }


        [HttpPost("años")]
        public async Task<IActionResult> ObtenerAños([FromBody] AñoRequest añoRequest)
        {
            try
            {
                await SetAuthorizationHeaderAsync();

                // Construir el cuerpo de la solicitud con el ID de la marca y modelo proporcionados
                var requestBody = new
                {
                    known_attributes = new[]
                    {
                        new
                        {
                            id = "BRAND",
                            value_id = añoRequest.MarcaId
                        },
                        new
                        {
                            id = "MODEL",
                            value_id = añoRequest.ModeloId
                        }
                    }
                };

                // Convertir el cuerpo de la solicitud a JSON
                var jsonBody = JsonSerializer.Serialize(requestBody);

                // Realizar la solicitud POST a la URL proporcionada
                var response = await _httpClient.PostAsync("https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/VEHICLE_YEAR/top_values",
                                                        new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json"));

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer y devolver la respuesta como una cadena JSON
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
                }
                else
                {
                    // Si la solicitud no fue exitosa, devolver un error con el código de estado
                    return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores generales
                return StatusCode(500, $"Error al realizar la solicitud: {ex.Message}");
            }
        }

        [HttpPost("catalogo/trim")]
        public async Task<IActionResult> CombinarAtributos([FromBody] CombinarAtributosRequest request)
        {
            try
            {
                await SetAuthorizationHeaderAsync();
                // Verificar si los identificadores necesarios están presentes en la solicitud
                if (string.IsNullOrEmpty(request.MarcaId) || string.IsNullOrEmpty(request.ModeloId) || string.IsNullOrEmpty(request.YearId))
                {
                    return BadRequest("Se requieren los identificadores de marca, modelo y trim en el cuerpo de la solicitud.");
                }

                // Construir el cuerpo de la solicitud con los identificadores proporcionados
                var requestBody = new
                {
                    known_attributes = new[]
                    {
                new
                {
                    id = "BRAND",
                    value_id = request.MarcaId
                },
                new
                {
                    id = "MODEL",
                    value_id = request.ModeloId
                },
                new
                {
                    id = "VEHICLE_YEAR",
                    value_id = request.YearId
                }
            }
                };

                // Convertir el cuerpo de la solicitud a JSON
                var jsonBody = JsonSerializer.Serialize(requestBody);

                // Realizar la solicitud POST a la URL proporcionada
                var response = await _httpClient.PostAsync("https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/trim/top_values",
                                                        new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json"));

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer y devolver la respuesta como una cadena JSON
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
                }
                else
                {
                    // Si la solicitud no fue exitosa, devolver un error con el código de estado
                    return StatusCode((int)response.StatusCode, $"La solicitud falló con el código de estado: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Manejar errores generales
                return StatusCode(500, $"Error al realizar la solicitud: {ex.Message}");
            }
        }


        [HttpPost("catalogo/combinar-atributos")]
        public async Task<IActionResult> CombinarAtributos()
        {
            try
            {
                await SetAuthorizationHeaderAsync();

                string endpoint = "https://api.mercadolibre.com/catalog_domains/MLA-CARS_AND_VANS/attributes/TRIM/top_values";

                HttpResponseMessage response = await _httpClient.PostAsync(endpoint, null);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return Ok(json);
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
        public class CombinarAtributosRequest
        {
            public string MarcaId { get; set; }
            public string ModeloId { get; set; }
            public string YearId { get; set; }
        }


        public class AñoRequest
        {
            public string MarcaId { get; set; }
            public string ModeloId { get; set; }
        }

        public class ModeloRequest
        {
            public string MarcaId { get; set; }
        }
    }
}
