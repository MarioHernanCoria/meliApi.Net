using MongoDB.Driver;

namespace meliApi.Data.Repositories
{
    public class MongoDBRepository
    {
        public MongoClient client;

        public IMongoDatabase db;

        public MongoDBRepository()
        {
            client = new MongoClient("mongodb://localhost:27017/");

            db = client.GetDatabase("Productos");
            db = client.GetDatabase("Item");
            db = client.GetDatabase("ItemPrices");
            db = client.GetDatabase("Message");
        }
    }
}
