using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sample.Auditing.Data
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var dbContext = new CatalogDbContext(new DbContextOptionsBuilder<CatalogDbContext>().UseSqlServer(
                new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.json"))
                    .Build()
                    .GetConnectionString("CatalogModelConnection")
                ).Options);

            dbContext.Database.Migrate();
            //new DataSeeder(dbContext).Seed();
            return dbContext;
        }
    }
}
