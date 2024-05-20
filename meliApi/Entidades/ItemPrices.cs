using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace meliApi.Entidades
{
    public class ItemPrices
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("resource")]
        public string Resource { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("topic")]
        public string Topic { get; set; }

        [Column("application_id")]
        public long ApplicationId { get; set; }

        [Column("attempts")]
        public int Attempts { get; set; }

        [Column("sent")]
        public DateTime Sent { get; set; }

        [Column("received")]
        public DateTime Received { get; set; }
    }
}
