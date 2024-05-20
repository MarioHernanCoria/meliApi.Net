using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        //private readonly MeliService _meliService;

        //public TokenController(MeliService meliService)
        //{
        //    _meliService = meliService;
        //}

        //[HttpPost("getCodeUrl")]
        //public async Task<IActionResult> GetCodeUrl()
        //{
        //    var key = await _meliService.GetCodeUrl();
        //    return StatusCode(201, key);
        //}

        //[HttpPost("getToken")]
        //public async Task<IActionResult> GetToken([FromBody] YourRequestBodyModel body)
        //{
        //    var data = await _meliService.GetToken(body.Code);
        //    return Ok(data);
        //}

        //[HttpPost("refreshToken")]
        //public async Task<IActionResult> RefreshToken([FromBody] YourRequestBodyModel body)
        //{
        //    var data = await _meliService.RefreshToken(body.Token);
        //    return Ok(data);
        //}

        //[HttpPost("meliHooks")]
        //public async Task<IActionResult> MeliHooks([FromBody] YourRequestBodyModel body)
        //{
        //    var data = await _meliService.ProcessEvent(body);
        //    return Ok(data);
        //}

        //[HttpPost("getServices")]
        //public async Task<IActionResult> GetServices([FromBody] YourRequestBodyModel body)
        //{
        //    var data = await _meliService.GetServices(body);
        //    return Ok(data);
        //}

        //[HttpPost("addService")]
        //public async Task<IActionResult> AddService([FromBody] YourRequestBodyModel body)
        //{
        //    var data = await _meliService.AddService(body);
        //    return Ok(data);
        //}

        //[HttpGet("getUser/{userId}")]
        //public async Task<IActionResult> GetUser(string userId)
        //{
        //    var data = await _meliService.GetUser(userId);
        //    return Ok(data);
        //}

        //[HttpGet("getProducts/{userId}")]
        //public async Task<IActionResult> GetProducts(string userId)
        //{
        //    var data = await _meliService.GetProducts(userId);
        //    return Ok(data);
        //}

        //[HttpGet("getUsers")]
        //public async Task<IActionResult> GetUsers([FromQuery] YourQueryParamsModel queryParams)
        //{
        //    var filter = queryParams.Name != null ? new Dictionary<string, string> { { "name", queryParams.Name } } : null;
        //    var options = queryParams.SortBy != null ? new Dictionary<string, string> { { "sortBy", queryParams.SortBy }, { "limit", queryParams.Limit }, { "page", queryParams.Page } } : null;
        //    var result = await _userService.QueryUsers(filter, options);
        //    return Ok(result);
        //}

        //[HttpPut("updateUser/{userId}")]
        //public async Task<IActionResult> UpdateUser(string userId, [FromBody] YourUpdateUserModel updateUserModel)
        //{
        //    var user = await _userService.UpdateUserById(userId, updateUserModel);
        //    return Ok(user);
        //}

        //[HttpDelete("deleteUser/{userId}")]
        //public async Task<IActionResult> DeleteUser(string userId)
        //{
        //    await _userService.DeleteUserById(userId);
        //    return NoContent();
        //}
    }
}
