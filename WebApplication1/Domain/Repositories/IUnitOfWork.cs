using MongoDB.Driver;
using System.Threading.Tasks;

namespace WebApplication1.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync(IClientSessionHandle session);
    }
}
