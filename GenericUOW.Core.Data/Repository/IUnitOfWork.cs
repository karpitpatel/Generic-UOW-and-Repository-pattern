using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Context;

namespace GenericUOW.Core.Data.Repository
{
    /// <summary>
    /// The core unit of work contract, to be used by repositories in order to
    /// coordinate access to (in this system) a common underlying data context.
    /// </summary>
   public interface IUnitOfWork : IDisposable
    {
        IDbContext Context { get; }

        int Save();
    }
}
