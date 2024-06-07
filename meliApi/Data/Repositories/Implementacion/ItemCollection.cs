//using meliApi.Data.Repositories;
//using meliApi.Data.Repositories.Interface;
//using meliApi.Entidades;
//using MongoDB.Bson;
//using MongoDB.Driver;

//namespace meliApi.Data.Repositories.Implementacion
//{
//    public class ItemCollection : IItemCollection
//    {

//        internal MongoDBRepository _repository = new MongoDBRepository();

//        private IMongoCollection<Item> Collection;

//        public ItemCollection()
//        {
//            Collection = _repository.db.GetCollection<Item>("Item");
//        }

//        public async Task DeleteItem(string id)
//        {
//            var filter = Builders<Item>.Filter.Eq(s => s.Id, new ObjectId(id));
//            await Collection.DeleteOneAsync(filter);
//        }

//        public async Task<List<Item>> GetAll()
//        {
//            return await Collection.FindAsync(new BsonDocument()).Result.ToListAsync();
//        }

//        public async Task<Item> GetProductoById(string id)
//        {
//            return await Collection.FindAsync(
//                new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstAsync();
//        }

//        public async Task InsertItem(Item item)
//        {
//            await Collection.InsertOneAsync(item);
//        }

//        public async Task UpdateItem(Item item)
//        {
//            var filter = Builders<Item>.Filter.Eq(s => s.Id, item.Id);
//            await Collection.ReplaceOneAsync(filter, item);
//        }
//    }
//}
