using Sample.Auditing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Auditing.Data.Repositories
{
    public interface IRepository<T> : IDisposable where T : BaseEntity
    {

        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByIdAsync(String id);

        IQueryable<T> Get();
        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        Task<Guid> InsertAsync(T item);
        Task InsertRangeAsync(IEnumerable<T> items);

        void Remove(Guid id);
        void RemoveRange(IEnumerable<Guid> ids);
    }

}
