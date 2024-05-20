using meliApi.DTOs;
using meliApi.Entidades;
using meliApi.Infraestructura;
using meliApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;



namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsuarioController> _logger;
        private readonly HttpClient _httpClient;
        public UsuarioController(IUnitOfWork unitOfWork, ILogger<UsuarioController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("api/usuarios/lista")]
        public async Task<IActionResult> ObtenerUsuariosConPermisos()
        {
            // Endpoint de la API de MercadoLibre
            string endpoint = "https://api.mercadolibre.com/users/233127985";

            try
            {
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
                    return InternalServerError(new Exception($"La solicitud falló con el código de estado: {response.StatusCode}"));
                }
            }
            catch (HttpRequestException ex)
            {
                // Manejar errores de solicitud HTTP
                return InternalServerError(ex);
            }
        }

        private IActionResult InternalServerError(Exception exception)
        {
            throw new NotImplementedException();
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Usuarios = await _unitOfWork.UsuarioRepository.GetAll();

                return ResponseFactory.CreateSuccessResponse(200, "Ok");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al obtener los usuarios.");
                return ResponseFactory.CreateErrorResponse(500, "Ocurrió un error interno.");
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var Usuario = await _unitOfWork.UsuarioRepository.GetById(id);
                if (Usuario == null)
                {
                    Log.Information("No se encontró ningún usuario con el ID {UserId}", id);

                }
                await _unitOfWork.Complete();

                Log.Information("Se encontro el Usuario");
                return ResponseFactory.CreateSuccessResponse(200, Usuario);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrió un error al obtener el usuario con ID {UserId}.", id);
                return ResponseFactory.CreateErrorResponse(500, "Ocurrió un error interno.");
            }

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _unitOfWork.UsuarioRepository.Delete(id);

            if (!result)
            {
                return ResponseFactory.CreateErrorResponse(500, "No se pudo eliminar el usuario");
            }
            else
            {
                await _unitOfWork.Complete();
                return ResponseFactory.CreateSuccessResponse(200, "Eliminado");

            }

        }
    }
}
