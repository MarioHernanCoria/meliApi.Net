using System.ComponentModel.DataAnnotations;

namespace meliApi.Entidades
{
    public class Usuario
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string Country_Id { get; set; }
        public Address Address { get; set; }
        public string User_Type { get; set; }
        public string Site_Id { get; set; }
        public string Permalink { get; set; }
        public SellerReputation Seller_Reputation { get; set; }
        public Status Status { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string State { get; set; }
    }

    public class SellerReputation
    {
        public object Level_Id { get; set; }
        public object Power_Seller_Status { get; set; }
        public Transactions Transactions { get; set; }
    }

    public class Transactions
    {
        public string Period { get; set; }
        public int Total { get; set; }
    }

    public class Status
    {
        public string Site_Status { get; set; }
    }
}
