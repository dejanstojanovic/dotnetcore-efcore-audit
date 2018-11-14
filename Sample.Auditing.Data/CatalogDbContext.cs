using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Sample.Auditing.Data.Entities;
using Sample.Auditing.Data.Entities.Configurations;

namespace Sample.Auditing.Data
{
    public class CatalogDbContext : DbContext
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly IHttpContextAccessor httpContextAccessor;


        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
         : base(options)
        {
        }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
          : base(options)
        {
            this.loggerFactory = loggerFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ProductConfiguration());
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var temoraryAuditProperies = await AuditNonTemporaryProperties();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await AuditTemporaryProperties(temoraryAuditProperies);
            return result;
        }


        async Task<IEnumerable<PropertyEntry>> AuditNonTemporaryProperties()
        {
            ChangeTracker.DetectChanges();
            var entitiesToTrack = ChangeTracker.Entries().Where(e => !(e.Entity is Audit) && e.State != EntityState.Detached && e.State != EntityState.Unchanged);

            await Audits.AddRangeAsync(
                entitiesToTrack.Select(e => new Audit()
                {
                    TableName = e.Metadata.Relational().TableName,
                    Action = Enum.GetName(typeof(EntityState), e.State),
                    Username = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name,
                    KeyValues = JsonConvert.SerializeObject(e.Properties.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)),
                    NewValues = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Added || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)),
                    OldValues = JsonConvert.SerializeObject(e.Properties.Where(p => e.State == EntityState.Deleted || e.State == EntityState.Modified).ToDictionary(p => p.Metadata.Name, p => p.OriginalValue))
                })
            );

            return entitiesToTrack.SelectMany(e => e.Properties).Where(p => p.IsTemporary);
        }

        async Task AuditTemporaryProperties(IEnumerable<PropertyEntry> temporatyPropertyEntries)
        {
            if (temporatyPropertyEntries != null && temporatyPropertyEntries.Any())
            {

                await Audits.AddRangeAsync(
                temporatyPropertyEntries.GroupBy(p => p.EntityEntry).Select(e => new Audit()
                    {
                        TableName = e.Key.Metadata.Relational().TableName,
                        Action = Enum.GetName(typeof(EntityState), e.Key.State),
                        Username = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name,
                        KeyValues = JsonConvert.SerializeObject(e.Where(p => p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)),
                        NewValues = JsonConvert.SerializeObject(e.Where(p => !p.Metadata.IsPrimaryKey()).ToDictionary(p => p.Metadata.Name, p => p.CurrentValue))
                    })
                );
                await SaveChangesAsync();
            }
            await Task.CompletedTask;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Audit> Audits { get; set; }
    }
}
