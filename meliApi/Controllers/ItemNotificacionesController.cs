using meliApi.Data.Repositories.Implementacion;
using meliApi.Data.Repositories.Interface;
using meliApi.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemNotificacionesController : ControllerBase
    {
        private IItemCollection db = new ItemCollection();

        [HttpGet]
        public async Task<IActionResult> GetAllItem()
        {
            return Ok(await db.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            return Ok(await db.GetProductoById(id));
        }


        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
            if (item == null)
            {
                return BadRequest();
            }


            await db.InsertItem(item);
            return Created("Created", true);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto([FromBody] Item item, string id)
        {
            if (item == null)
            {
                return BadRequest();
            }

            item.Id = new MongoDB.Bson.ObjectId(id);
            await db.UpdateItem(item);
            return Created("Created", true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await db.DeleteItem(id);
            return NoContent();
        }
    }
}