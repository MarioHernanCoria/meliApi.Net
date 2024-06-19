using meliApi.Data.Repositories.Implementacion;
using meliApi.Data.Repositories.Interface;
using meliApi.Entidades;
using meliApi.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace meliApi.Controllers
{
    [Route("api/callback")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly IItemCollection _db;
        private readonly ILogger<NotificacionesController> _logger;
        private readonly HttpClient _httpClient;
        private readonly TokenServicios _tokenServicios;
        private readonly string _appId;

        public NotificacionesController(IItemCollection itemCollection, ILogger<NotificacionesController> logger, TokenServicios tokenServicios, HttpClient httpClient)
        {
            _db = itemCollection;
            _logger = logger;
            _tokenServicios = tokenServicios;
            _httpClient = httpClient;
            _appId = "86626730255656"; // Reemplaza con tu ID de aplicación
        }

        [HttpPost]
        public async Task<IActionResult> SubNotifications()
        {
            try
            {

                await SetAuthorizationHeaderAsync();

                await SubscribeToNotificationsAsync("items", "https://admin.publicar.shop/meli/api/callback");
                return Ok("Suscripción a notificaciones de items exitosa.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al suscribirse a las notificaciones");
                return StatusCode(500, $"Error al suscribirse a las notificaciones: {ex.Message}");
            }
        }

        [HttpPost("recibe")]
        public IActionResult ReceiveNotification([FromBody] NotificationPayload payload)
        {
            try
            {
                SetAuthorizationHeaderAsync();

                _logger.LogInformation("Received notification: {Payload}", payload);

                // Procesar la notificación aquí, por ejemplo, guardar en la base de datos

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la notificación");
                return StatusCode(500, $"Error al procesar la notificación: {ex.Message}");
            }
        }

        private async Task SubscribeToNotificationsAsync(string topic, string callbackUrl)
        {
            await SetAuthorizationHeaderAsync();

            string endpoint = $"https://api.mercadolibre.com/applications/{_appId}/notifications";
            var webhookData = new
            {
                topic = topic,
                callback_url = callbackUrl
            };

            var json = JsonConvert.SerializeObject(webhookData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error al suscribirse a las notificaciones: {StatusCode} - {Content}", response.StatusCode, errorContent);
                throw new InvalidOperationException($"Error al suscribirse a las notificaciones: {response.StatusCode} - {errorContent}");
            }
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            try
            {
                var tokenData = await _tokenServicios.ObtenerUltimoToken();

                if (tokenData.expires_in < 21600)
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

        public class NotificationPayload
        {
            public string _id { get; set; }
            public string resource { get; set; }
            public long user_id { get; set; }
            public string topic { get; set; }
            public long application_id { get; set; }
            public int attempts { get; set; }
            public string sent { get; set; }
            public string received { get; set; }
        }
    }
}
