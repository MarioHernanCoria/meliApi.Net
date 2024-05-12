﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace meliApi.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ItemsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "APP_USR-86626730255656-051204-54755567add5497d0d03959f559ccab2-233127985");
        }

        [HttpGet("listaDeItems")]
        public async Task<IActionResult> ObtenerListaDeItemsPorUsuario()
        {
            try
            {
                // Endpoint para obtener la lista de ítems del usuario
                string endpoint = $"https://api.mercadolibre.com/users/233127985/items/search";

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



        [HttpGet("UnItem")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItem()
        {
            try
            {
                // Endpoint para obtener especificaciones de un ítem específico
                string endpoint = $"https://api.mercadolibre.com/items/MLA850821971";

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

        [HttpGet("variosItems")]
        public async Task<IActionResult> ObtenerEspecificacionesDeItems()
        {
            try
            {
                // Endpoint para obtener especificaciones de varios ítems específicos
                string endpoint = $"https://api.mercadolibre.com/items?ids=MLA850821971,MLA851221877";

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
    }
}