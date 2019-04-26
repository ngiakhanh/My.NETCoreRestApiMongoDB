using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Persistence.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly IMongoCollection<Product> _product;
        public ProductRepository(IConfiguration config) : base(config)
        {
            _product = _database.GetCollection<Product>("Product");
        }
        public async Task AddAsync(Product product)
        {
            await _product.InsertOneAsync(product);
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _product.Find(one => one.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> ListAsync()
        {
            var results = await _product.Aggregate().Lookup("Category", "CategoryId", "Id", "Category").ToListAsync();
            return BsonSerializer.Deserialize<IEnumerable<Product>>(results.ToJson()); ;
        }

        public async Task RemoveAsync(int id)
        {
            await _product.DeleteOneAsync(one => one.Id == id);
        }

        public async Task UpdateAsync(int id, Product product)
        {
            await _product.ReplaceOneAsync(one => one.Id == id, product);
        }
    }
}
