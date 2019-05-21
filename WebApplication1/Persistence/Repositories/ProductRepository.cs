using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Persistence.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly IMongoCollection<BsonDocument> _product;
        public ProductRepository(IConfiguration config) : base(config)
        {
            _product = _database.GetCollection<BsonDocument>("Product");
        }

        public IClientSessionHandle ReturnSession()
        {
            return _session;
        }

        public async Task AddAsync(Product product)
        {
            await _product.InsertOneAsync(_session, product.ToBsonDocument());
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            var match = new BsonDocument
            {
                {
                    "Id", id
                }
            };
            var result = await _product.Aggregate(_session).Lookup("Category", "CategoryId", "Id", "Category").Match(one => one["Id"] == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            return BsonSerializer.Deserialize<Product>(result.ToJson());
        }

        public async Task<IEnumerable<Product>> ListAsync()
        {
            var results = await _product.Aggregate(_session).Lookup("Category", "CategoryId", "Id", "Category").SortBy(one => one["Id"]).ToListAsync();
            if (results == null)
            {
                return null;
            }
            return BsonSerializer.Deserialize<IEnumerable<Product>>(results.ToJson());
        }

        public async Task RemoveAsync(int id)
        {
            await _product.DeleteOneAsync(_session, one => one["Id"] == id);
        }

        public async Task UpdateAsync(int id, Product product)
        {
            await _product.ReplaceOneAsync(_session, one => one["Id"] == id, product.ToBsonDocument());
        }
    }
}