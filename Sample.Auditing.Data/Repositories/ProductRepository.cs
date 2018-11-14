using Microsoft.EntityFrameworkCore;
using Sample.Auditing.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Auditing.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
