using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace meliApi.Entidades
{
    public class Producto
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Categoria { get; set; }   
    }
}
