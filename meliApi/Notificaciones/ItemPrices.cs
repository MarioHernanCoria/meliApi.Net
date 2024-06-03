using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace meliApi.Notificaciones
{
    public class ItemPrices
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Resource { get; set; }
        public long UserId { get; set; }
        public string Topic { get; set; }
        public long ApplicationId { get; set; }
        public int Attempts { get; set; }
        public DateTime Sent { get; set; }
        public DateTime Received { get; set; }
    }
}
