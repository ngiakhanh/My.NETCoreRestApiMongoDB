using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;
using WebApplication1.Persistence.Context;

namespace WebApplication1.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        private readonly IMongoCollection<Category> _category;
        public CategoryRepository(IConfiguration config) : base(config)
        {
            _category = _database.GetCollection<Category>("Category");
        }
        public async Task AddAsync(Category category)
        {
            await _category.InsertOneAsync(category);
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            return await _category.Find(one => one.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _category.Find(one => true).ToListAsync();
        }

        public async Task RemoveAsync(int id)
        {
            await _category.DeleteOneAsync(one => one.Id == id);
        }

        public async Task UpdateAsync(int id, Category category)
        {
            await _category.ReplaceOneAsync(one => one.Id == id, category);
        }
    }
}
