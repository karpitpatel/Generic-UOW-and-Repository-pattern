using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenericUOW.Core.Data.Repository
{
    /// <summary>
    /// The core repository type, offering basic SCRUD operations against a
    /// particular type of entity.
    /// </summary>
    /// <typeparam name="TEntity">the entity type to be managed</typeparam>
    public interface IRepository<TEntity>
           where TEntity : class
    {
        IUnitOfWork UnitOfWork { get; }

        void Delete(object id);

        void Delete(TEntity entity);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "EF convention")]
        TEntity Get(object id);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "EF convention")]
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            IEnumerable<string> propertyIncludes = null,
            int? page = null,
            int? pageSize = null);

        void Insert(TEntity entity);

        void InsertGraph(TEntity entity);

        void Update(TEntity entity);
    }
}
