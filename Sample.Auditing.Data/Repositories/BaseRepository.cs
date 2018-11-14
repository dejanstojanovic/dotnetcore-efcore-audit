using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Auditing.Data.Entities;

namespace Sample.Auditing.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {

        DbContext dbContext;
        bool disposing;

        public DbContext DbContext
        {
            get
            {
                return this.dbContext;
            }
        }

        public BaseRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual void Dispose()
        {
            if (!this.disposing)
            {
                this.dbContext.Dispose();
                this.disposing = true;
            }
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await this.dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await GetByIdAsync(Guid.Parse(id));
        }

        public virtual IQueryable<T> Get()
        {
            return this.dbContext.Set<T>().AsNoTracking();
        }

        public virtual async Task<Guid> InsertAsync(T item)
        {
            await this.dbContext.Set<T>().AddAsync(item);
            return item.Id;
        }

        public virtual IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return this.dbContext.Set<T>().AsNoTracking().Where(expression);
        }

        public void Remove(Guid id)
        {
            var item = this.dbContext.Set<T>().FirstOrDefault(e => e.Id == id);
            if (item != null)
            {
                this.dbContext.Set<T>().Remove(item);
            }
        }

        public void RemoveRange(IEnumerable<Guid> ids)
        {
            var items = this.dbContext.Set<T>().Where(e => ids.Contains(e.Id));
            if (items != null && items.Any())
            {
                this.dbContext.Set<T>().RemoveRange(items);
            }
        }

        public async Task InsertRangeAsync(IEnumerable<T> items)
        {
            await this.dbContext.Set<T>().AddRangeAsync(items);
        }


    }

}
