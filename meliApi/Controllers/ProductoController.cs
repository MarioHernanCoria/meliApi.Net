using meliApi.Data.Repositories.Implementacion;
using meliApi.Data.Repositories.Interface;
using meliApi.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace meliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private IProductosCollection db = new ProductoCollection();

        [HttpGet]
        public async Task<IActionResult> GetAllProcuctos()
        {
            return Ok(await db.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto(string id)
        {
            return Ok(await db.GetProductoById(id));
        }


        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] Producto producto)
        {
            if (producto == null) 
            {
                return BadRequest();
            }

            if(producto.Name == string.Empty) 
            {
                ModelState.AddModelError("Name", "El producto esta vacio");
            }

            await db.InsertProducto(producto);
            return Created("Created", true);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto([FromBody] Producto producto, string id)
        {
            if (producto == null)
            {
                return BadRequest();
            }

            if (producto.Name == string.Empty)
            {
                ModelState.AddModelError("Name", "El producto esta vacio");
            }
            producto.Id = new MongoDB.Bson.ObjectId(id);
            await db.UpdateProducto(producto);
            return Created("Created", true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(string id)
        {
            await db.DeleteProducto(id);
            return NoContent(); 
        }
    }
}
