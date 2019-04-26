using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Threading.Tasks;
using WebApplication1.Domain.Models;
using WebApplication1.Domain.Repositories;
using WebApplication1.Persistence.Context;

namespace WebApplication1.Persistence.Repositories
{

    public class UnitOfWork : BaseRepository, IUnitOfWork
    {
        private readonly IMongoCollection<Category> _category;

        public UnitOfWork(IConfiguration config) : base(config)
        {
            _category = _database.GetCollection<Category>("Category");
        }

        public async Task CompleteAsync()
        {
            //await _category.SaveChangesAsync();
        }
    }
}
