using meliApi.Data.Repositories;
using meliApi.Data.Repositories.Interface;
using meliApi.Entidades;
using MongoDB.Bson;
using MongoDB.Driver;

namespace meliApi.Data.Repositories.Implementacion
{
    public class ProductoCollection : IProductosCollection
    {
        internal MongoDBRepository _repository = new MongoDBRepository();

        private IMongoCollection<Producto> Collection;

        public ProductoCollection()
        {
            Collection = _repository.db.GetCollection<Producto>("Productos");
        }

        public async Task DeleteProducto(string id)
        {
            var filter = Builders<Producto>.Filter.Eq(s => s.Id.ToString(), id);

            await Collection.DeleteOneAsync(filter);
        }

        public async Task<List<Producto>> GetAll()
        {
            return await Collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        public async Task<Producto> GetProductoById(string id)
        {
            return await Collection.FindAsync(
                new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstAsync();
        }

        public async Task InsertProducto(Producto producto)
        {
            await Collection.InsertOneAsync(producto);
        }

        public async Task UpdateProducto(Producto producto)
        {
            var filter = Builders<Producto>.Filter.Eq(s => s.Id, producto.Id);
            await Collection.ReplaceOneAsync(filter, producto);
        }
    }
}
