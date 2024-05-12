using System.ComponentModel.DataAnnotations;

namespace meliApi.Entidades
{
    public class SellerReputation
    {
        public object LevelId { get; set; }
        public object PowerSellerStatus { get; set; }
        public Transactions Transactions { get; set; }
    }
}
