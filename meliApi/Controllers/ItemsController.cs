using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using meliApi.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace meliApi.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly TokenServicios _tokenServicios;

        public ItemsController(IHttpClientFactory httpClientFactory, TokenServicios tokenServicios)
        {
            _httpClient = httpClientFactory.CreateClient();
            _tokenServicios = tokenServicios;
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

        [HttpGet("me")]
        public async Task<IActionResult> ObtenerListaDeItemsPorUsuario()
        {
            try
            {

                await SetAuthorizationHeaderAsync();

                string endpoint = $"https://api.mercadolibre.com/users/me";
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

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



        [HttpGet("UnItem")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItem()
        {
            try
            {
                await SetAuthorizationHeaderAsync();
                string endpoint = $"https://api.mercadolibre.com/items/MLA1423103713";

                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

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

        [HttpGet("variosItems")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems()
        {
            try
            {

                await SetAuthorizationHeaderAsync();
                string endpoint = $"https://api.mercadolibre.com/items?ids=MLA1423103713,MLA879778758,MLA879778265,MLA879777318";
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

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





    }
}