using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Auditing.Data.Entities;
using Sample.Auditing.Data.Repositories;

namespace Sample.Auditing.Data.UnitsOfWork
{
    public class ProductCatalogUnitOfWork : IUnitOfWork
    {
        CatalogDbContext dbContext;
        bool disposing;

        public IDatabaseTransaction BeginTransaction => new EntityDatabaseTransaction(this.dbContext);

        public ProductRepository Products { get; private set; }

        public ProductCatalogUnitOfWork(CatalogDbContext dbContext)
        {
            this.disposing = false;
            this.dbContext = dbContext;
            this.Products = new ProductRepository(dbContext);
        }



            public void Update<T>(T entity) where T : BaseEntity
        {
            this.dbContext.Set<T>().Update(entity);
        }

        public int Save()
        {
            return this.dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await this.dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            if (!this.disposing)
            {
                this.dbContext.Dispose();
                this.disposing = true;
            }
        }

    }
}
