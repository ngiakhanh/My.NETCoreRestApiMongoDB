﻿using MongoDB.Driver;
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

        public async Task AbortAsync(IClientSessionHandle session)
        {
            await session.AbortTransactionAsync();
        }

        public void StartTransaction(IClientSessionHandle session)
        {
            session.StartTransaction(new TransactionOptions(readConcern: ReadConcern.Snapshot, writeConcern: WriteConcern.WMajority));
        }

        public void EndSession(IClientSessionHandle session)
        {
            if (!session.IsInTransaction)
            {
                session.Dispose();
            }
        }
    }
}