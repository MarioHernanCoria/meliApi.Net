//using meliApi.Data.Repositories.Interface;
//using meliApi.Entidades;
//using MongoDB.Bson;
//using MongoDB.Driver;

//namespace meliApi.Data.Repositories.Implementacion
//{
//    public class MessageCollection : IMessageCollection
//    {
//        internal MongoDBRepository _repository = new MongoDBRepository();

//        private IMongoCollection<Message> Collection;

//        public MessageCollection()
//        {
//            Collection = _repository.db.GetCollection<Message>("Message");
//        }

//        public async Task DeleteMessage(string id)
//        {
//            var filter = Builders<Message>.Filter.Eq(s => s.Id, new ObjectId(id));
//            await Collection.DeleteOneAsync(filter);
//        }

//        public async Task<List<Message>> GetAll()
//        {
//            return await Collection.FindAsync(new BsonDocument()).Result.ToListAsync();
//        }

//        public async Task<Message> GetProductoById(string id)
//        {
//            return await Collection.FindAsync(
//                new BsonDocument { { "_id", new ObjectId(id) } }).Result.FirstAsync();
//        }

//        public async Task InsertMessage(Message message)
//        {
//            await Collection.InsertOneAsync(message);
//        }

//        public async Task UpdateMessage(Message message)
//        {
//            var filter = Builders<Message>.Filter.Eq(s => s.Id, message.Id);
//            await Collection.ReplaceOneAsync(filter, message);
//        }
//    }
//}
