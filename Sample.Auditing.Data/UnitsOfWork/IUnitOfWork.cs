using Sample.Auditing.Data.Entities;
using Sample.Auditing.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auditing.Data.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ProductRepository Products { get; }

        void Update<T>(T entity) where T : BaseEntity;

        int Save();
        Task<int> SaveAsync();

        IDatabaseTransaction BeginTransaction { get; }
    }

}
