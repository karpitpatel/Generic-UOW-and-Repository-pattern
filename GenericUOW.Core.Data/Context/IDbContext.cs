using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericUOW.Core.Data.Context
{
    public interface IDbContext : IDisposable
    {
        int SaveChanges();

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Set", Justification = "EF convention")]
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
