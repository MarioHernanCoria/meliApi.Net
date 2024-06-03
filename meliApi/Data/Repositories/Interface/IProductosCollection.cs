using meliApi.Entidades;

namespace meliApi.Data.Repositories.Interface
{
    public interface IProductosCollection
    {
        Task InsertProducto(Producto producto);
        Task DeleteProducto(string id);
        Task UpdateProducto(Producto producto);
        Task<Producto> GetProductoById(string id);
        Task<List<Producto>> GetAll();
    }
}
