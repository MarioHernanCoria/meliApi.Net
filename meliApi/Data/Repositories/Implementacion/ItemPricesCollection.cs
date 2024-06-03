using meliApi.Data.Repositories.Interface;
using meliApi.Notificaciones;
using MongoDB.Bson;
using MongoDB.Driver;

namespace meliApi.Data.Repositories.Implementacion
{
    public class ItemPricesCollection : IItemPricesCollection
    {
        internal MongoDBRepository _repository = new MongoDBRepository();

        private IMongoCollection<ItemPrices> Collection;

        public ItemPricesCollection()
        {
            Collection = _repository.db.GetCollection<ItemPrices>("ItemPrices");
        }

        public async Task DeleteItemPrices(string id)
        {
            var filter = Builders<ItemPrices>.Filter.Eq(s => s.Id, new ObjectId(id));
            await Collection.DeleteOneAsync(filter);
        }

        public async Task<List<ItemPrices>> GetAll()
        {
            return await Collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        public async Task<ItemPrices> GetProductoById(string id)
        {
            return await Collection.FindAsync(
                new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstAsync();
        }

        public async Task InsertItemPrices(ItemPrices message)
        {
            await Collection.InsertOneAsync(message);
        }

        public async Task UpdateItemPrices(ItemPrices message)
        {
            var filter = Builders<ItemPrices>.Filter.Eq(s => s.Id, message.Id);
            await Collection.ReplaceOneAsync(filter, message);
        }
    }
}
