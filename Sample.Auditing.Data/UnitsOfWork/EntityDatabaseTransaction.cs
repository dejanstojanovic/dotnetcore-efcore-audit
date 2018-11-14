using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sample.Auditing.Data.UnitsOfWork
{
    class EntityDatabaseTransaction : IDatabaseTransaction
    {
        private IDbContextTransaction transaction;

        public EntityDatabaseTransaction(DbContext context)
        {
            transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            transaction.Dispose();
        }
    }

}
