using MongoDB.Driver;
using System.Threading.Tasks;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Persistence.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {
        //protected readonly IClientSessionHandle _session;

        public UnitOfWork()
        {

        }

        public async Task CompleteAsync(IClientSessionHandle session)
        {
            await session.CommitTransactionAsync();
        }
    }
}
