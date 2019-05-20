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
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        private readonly IMongoCollection<BsonDocument> _category;
        public CategoryRepository(IConfiguration config) : base(config)
        {
            _category = _database.GetCollection<BsonDocument>("Category");
        }

        public IClientSessionHandle ReturnSession()
        {
            return _session;
        }

        public async Task AddAsync(Category category)
        {
            await _category.InsertOneAsync(category.ToBsonDocument());
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            var match = new BsonDocument
            {
                {
                    "Id", id
                }
            };
            var result = await _category.Aggregate().Lookup("Product", "Id", "CategoryId", "Products").Match(one => one["Id"] == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }
            return BsonSerializer.Deserialize<Category>(result.ToJson());
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            var results = await _category.Aggregate().Lookup("Product", "Id", "CategoryId", "Products").SortBy(one => one["Id"]).ToListAsync();
            if (results == null)
            {
                return null;
            }
            return BsonSerializer.Deserialize<IEnumerable<Category>>(results.ToJson());
        }

        public async Task RemoveAsync(int id)
        {
            await _category.DeleteOneAsync(one => one["Id"] == id);
        }

        public async Task UpdateAsync(int id, Category category)
        {
            await _category.ReplaceOneAsync(one => one["Id"] == id, category.ToBsonDocument());
        }
    }
}
