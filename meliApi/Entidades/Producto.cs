using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace meliApi.Entidades
{
    public class Producto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Ids { get; set; }
        public string Id { get; set; }

        public string site_id { get; set; }
        public string title { get; set; }
        public int seller_id { get; set; }
        public string category_id { get; set; }
        public string user_product_id { get; set; }
        public int? official_store_id { get; set; }
        public decimal price { get; set; }
        public decimal base_price { get; set; }
        public decimal? original_price { get; set; }
        public string inventory_id { get; set; }
        public string currency_id { get; set; }
        public int initial_quantity { get; set; }
        public int available_quantity { get; set; }
        public int sold_quantity { get; set; }
        public List<SaleTerm> sale_terms { get; set; }
        public string buying_mode { get; set; }
        public string listing_type_id { get; set; }
        public DateTime start_time { get; set; }
        public DateTime stop_time { get; set; }
        public DateTime end_time { get; set; }
        public DateTime expiration_time { get; set; }
        public string condition { get; set; }
        public string permalink { get; set; }
        public string thumbnail_id { get; set; }
        public string thumbnail { get; set; }
        public List<Picture> pictures { get; set; }
        public string video_id { get; set; }
        public List<string> descriptions { get; set; }
        public bool accepts_mercadopago { get; set; }
        public List<string> non_mercado_pago_payment_methods { get; set; }
        public Shipping shipping { get; set; }
        public string international_delivery_mode { get; set; }
        public SellerAddress seller_address { get; set; }
        public object seller_contact { get; set; }
        public Location location { get; set; }
        public Geolocation geolocation { get; set; }
        public List<object> coverage_areas { get; set; }
        public List<Attribute> attributes { get; set; }
        public List<object> warnings { get; set; }
        public string listing_source { get; set; }
        public List<object> variations { get; set; }
        public string status { get; set; }
        public List<string> sub_status { get; set; }
        public List<string> tags { get; set; }
        public string warranty { get; set; }
        public string catalog_product_id { get; set; }
        public string domain_id { get; set; }
        public object seller_custom_field { get; set; }
        public string parent_item_id { get; set; }
        public object differential_pricing { get; set; }
        public List<string> deal_ids { get; set; }
        public bool automatic_relist { get; set; }
        public DateTime date_created { get; set; }
        public DateTime last_updated { get; set; }
        public double health { get; set; }
        public bool catalog_listing { get; set; }
        public List<object> item_relations { get; set; }
        public List<string> channels { get; set; }
    }

    public class SaleTerm
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value_id { get; set; }
        public string value_name { get; set; }
        public object value_struct { get; set; }
        public List<Value> values { get; set; }
        public string value_type { get; set; }
    }

    public class Value
    {
        public string id { get; set; }
        public string name { get; set; }
        public object structValue { get; set; }
    }

    public class Picture
    {
        public string id { get; set; }
        public string url { get; set; }
        public string secure_url { get; set; }
        public string size { get; set; }
        public string max_size { get; set; }
        public string quality { get; set; }
    }

    public class Shipping
    {
        public string mode { get; set; }
        public List<object> methods { get; set; }
        public List<string> tags { get; set; }
        public object dimensions { get; set; }
        public bool local_pick_up { get; set; }
        public bool free_shipping { get; set; }
        public string logistic_type { get; set; }
        public bool store_pick_up { get; set; }
    }

    public class SellerAddress
    {
        public string address_line { get; set; }
        public string zip_code { get; set; }
        public City city { get; set; }
        public State state { get; set; }
        public Country country { get; set; }
        public SearchLocation search_location { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int id { get; set; }
    }

    public class City
    {
        public string name { get; set; }
    }

    public class State
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Country
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class SearchLocation
    {
        public City city { get; set; }
        public State state { get; set; }
    }

    public class Location
    {
    }

    public class Geolocation
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Attribute
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value_id { get; set; }
        public string value_name { get; set; }
        public List<Value> values { get; set; }
        public string value_type { get; set; }
    }
}
