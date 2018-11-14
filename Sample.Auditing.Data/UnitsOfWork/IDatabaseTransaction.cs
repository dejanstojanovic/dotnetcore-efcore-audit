using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Auditing.Data.UnitsOfWork
{
    public interface IDatabaseTransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }

}
