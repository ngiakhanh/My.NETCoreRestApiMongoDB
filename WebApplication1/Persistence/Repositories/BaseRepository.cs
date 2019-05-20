using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace WebApplication1.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IMongoDatabase _database;
        protected readonly IClientSessionHandle _session;
        public BaseRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("Test"));
            _session = client.StartSession();
            _database = _session.Client.GetDatabase("Test");
            _session.StartTransaction();
        }
    }
}
