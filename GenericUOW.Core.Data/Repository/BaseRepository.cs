using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Model;

namespace GenericUOW.Core.Data.Repository
{
    /// <summary>
    /// A base implementation of IRepository, providing access to a particular
    /// type of entity. Repositories are tied to an underlying unit of work that
    /// coordinates data access between them, and this base implementation offers
    /// a standard set of SCRUD operations. Subclasses will extend this functionality
    /// with their own entity-specific members.
    /// </summary>
    /// <typeparam name="TEntity">the type of entity to manage here</typeparam>
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
           where TEntity : BaseEntity
    {
        private readonly IDbSet<TEntity> dataSet;

        /// <summary>
        /// Creates a new BaseRepository for the specified entity type
        /// around the given unit of work.
        /// </summary>
        /// <param name="unitOfWork">the UOW instance underlying this repository</param>
        protected BaseRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");

            this.dataSet = unitOfWork.Context.Set<TEntity>();
            this.UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// Accessor to the unit of work underlying this repository, and through
        /// which changes can be persisted.
        /// </summary>
        public IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Provides access to the underlying dataset of this repository, for
        /// customised use by subclasses.
        /// </summary>
        protected IDbSet<TEntity> DataSet
        {
            get { return dataSet; }
        }

        /// <summary>
        /// Removes the item with the given primary key from this repository.
        /// </summary>
        /// <param name="id">the ID of the item to be removed</param>
        public virtual void Delete(object id)
        {
            var entity = dataSet.Find(id);
            Delete(entity);
        }

        /// <summary>
        /// Removes the item provided from this repository.
        /// </summary>
        /// <param name="entity">the item to be removed</param>
        public virtual void Delete(TEntity entity)
        {
            if (entity != null)
            {
                entity.ObjectState = ObjectState.Deleted;
                dataSet.Remove(entity);
            }
        }

        /// <summary>
        /// Attempts to find and return an item with the given primary key.
        /// </summary>
        /// <param name="id">the ID of the item to be returned</param>
        /// <returns>the item found</returns>
        public virtual TEntity Get(object id)
        {
            return dataSet.Find(id);
        }

        /// <summary>
        /// Uses the given filter expression, ordering statement, and inclusion
        /// list to obtain a subset of the items in this repository. Paging is
        /// also supported, if required.
        /// </summary>
        /// <param name="filter">an expression on the type of items stored here to filter by</param>
        /// <param name="orderBy">the means by which to order the result</param>
        /// <param name="propertyIncludes">an optional set of property names to include in the result</param>
        /// <param name="page">the zero-based index of the page/set of results to return</param>
        /// <param name="pageSize">the size of a page set when using paging</param>
        /// <returns>the subset of items that satisfy this restriction, formatted accordingly</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            IEnumerable<string> propertyIncludes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> result = dataSet;

            if (propertyIncludes != null)
            {
                result = propertyIncludes.Aggregate(result, (current, property) => current.Include(property));
            }

            if (filter != null)
            {
                result = result.Where(filter);
            }

            if (orderBy != null)
            {
                result = orderBy(result);
            }

            if (page != null && pageSize != null && result != null && result.Any())
            {
                result = result.Skip(page.Value * pageSize.Value)
                               .Take(pageSize.Value);
            }

            return result;
        }

        /// <summary>
        /// Adds the given item to this repository. A shallow insert, which
        /// only marks the given entity as added, without consideration for
        /// the rest of its graph.
        /// </summary>
        /// <param name="entity">the item to be added</param>
        public virtual void Insert(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;
            dataSet.Attach(entity);
        }

        /// <summary>
        /// Adds the given item to this repository. This is a deep insert that
        /// marks the given entity, and all associated entities in its graph,
        /// as having been added.
        /// </summary>
        /// <param name="entity">the item to be added</param>
        public virtual void InsertGraph(TEntity entity)
        {
            if (entity != null)
            {
                entity.ObjectState = ObjectState.Added;
                dataSet.Add(entity);
            }
        }

        /// <summary>
        /// Applies updates contained within the given item to an existing item
        /// in this repository.
        /// </summary>
        /// <param name="entity">contains updates to an existing item</param>
        public virtual void Update(TEntity entity)
        {
            if (entity != null)
            {
                entity.ObjectState = ObjectState.Modified;
                dataSet.Attach(entity);
            }
        }
    }
}
