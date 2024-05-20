using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoConnection");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("meliapi");
    }

    // Aquí puedes agregar propiedades para acceder a tus colecciones MongoDB
}
