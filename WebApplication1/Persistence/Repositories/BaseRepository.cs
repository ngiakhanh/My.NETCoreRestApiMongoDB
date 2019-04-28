using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace WebApplication1.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IMongoDatabase _database;
        public BaseRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("Test"));
            _database = client.GetDatabase("Test");
        }
    }
}
