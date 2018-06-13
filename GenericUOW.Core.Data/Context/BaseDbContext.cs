using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Model;

namespace GenericUOW.Core.Data.Context
{
    /// <summary>
    /// A generic context underlying all other contexts used in the membership
    /// system, implementing common/core functionality.
    ///
    /// This functionality currently centres around the state tracking of
    /// entities against disconnected contexts. All entities identified as
    /// IStateEntities have their state initialised on materialization, and
    /// consolidated with their container entries prior to being saved.
    /// This implements a simple entity-based state tracking system that
    /// prevents conflicts when persisting entities between contexts.
    ///
    /// Note that while all entities in the system will derive from IStateEntity,
    /// change-synchronisation is implemented on an "opt in" paradigm, in which
    /// entities that don't set their local state will be persisted in the
    /// normal way, without synchronisation.
    /// </summary>
    public abstract class BaseDbContext : DbContext, IDbContext
    {
        public const string ErrorInvalidObjectType = "Type {0} does not implement IStateEntity";
        public const string ErrorEntityValidation = "Validation error for entity of type '{0}' in state '{1}': [property={2}, error={3}]";

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected BaseDbContext()
        {
        }

        /// <summary>
        /// Creates a new BaseDbContext around the given connection string
        /// </summary>
        /// <param name="connectionString">a connection string to the database</param>
        protected BaseDbContext(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Overridden to ensure that any tracked entities that have explicitly
        /// set their local state will have this state synchronised to their
        /// context entry prior to being saved. Entities that have not used this
        /// tracking state are not affected.
        /// </summary>
        /// <returns>the number of items affected by the save</returns>
        public override int SaveChanges()
        {
            var stateEntities = new List<IStateEntity>();
            foreach (var entry in ChangeTracker.Entries())
            {
                // Ensure that any IStateEntity with an ObjectState that has
                // been explicitly set has its entity state synchronised.
                var stateEntity = entry.Entity as IStateEntity;

                if (stateEntity == null)
                {
                    throw new InvalidCastException(string.Format(ErrorInvalidObjectType, entry.Entity.GetType().AssemblyQualifiedName));
                }

                stateEntities.Add(stateEntity);

                if (stateEntity.ObjectState != ObjectState.Unknown)
                {
                    entry.State = stateEntity.GetEntityState();
                }

                // If this is an entity with timestamp data, update the
                // last modified and creation times, if required.
                var baseEntity = entry.Entity as BaseEntity;

                if (baseEntity != null)
                {
                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                        baseEntity.Modified = DateTime.UtcNow;

                    if (entry.State == EntityState.Added ||
                        baseEntity.ObjectState == ObjectState.Added ||
                        baseEntity.Created == DateTime.MinValue)
                    {
                        baseEntity.Created = baseEntity.Modified;
                    }
                }
            }

            try
            {
                var result = 0;
                bool saveFailed;

                do
                {
                    saveFailed = false;

                    try
                    {
                        result = base.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }
                }
                while (saveFailed);

                foreach (var stateEntity in stateEntities)
                {
                    stateEntity.ObjectState = ObjectState.Unchanged;
                }

                return result;
            }
            catch (DbEntityValidationException e)
            {
                var validationExceptions = (from entityValidationError in e.EntityValidationErrors
                                            from dbValidationError in entityValidationError.ValidationErrors
                                            select new Exception(GetExceptionMessage(entityValidationError, dbValidationError))).ToList();
                validationExceptions.Add(e);
                throw new AggregateException(validationExceptions);
            }
        }

        /// <summary>
        /// Retreives a set of items of the type specified.
        /// </summary>
        /// <typeparam name="TEntity">the type of items to return</typeparam>
        /// <returns>the set of this type of item currently in the context</returns>
        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return Set<TEntity>();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private static string GetExceptionMessage(
            DbEntityValidationResult entityValidationError,
            DbValidationError databaseValidationError)
        {
            return string.Format(
                ErrorEntityValidation,
                entityValidationError.Entry.Entity.GetType().Name,
                entityValidationError.Entry.State,
                databaseValidationError.PropertyName,
                databaseValidationError.ErrorMessage);
        }
    }
}
