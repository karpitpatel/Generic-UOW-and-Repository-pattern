using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericUOW.Core.Data.Model
{
    /// <summary>
    /// The base of all entities in the membership system. All such entities
    /// are instances of IStateEntity by implication, so that they have the
    /// ability to track & signal their own persistence state between contexts
    /// if required. Note that the implementation of BaseDbContext is such that
    /// this is not a requirement - entities that do not have cross-context
    /// persistence concerns need not utilise these properties.
    /// </summary>
    public class BaseEntity : IStateEntity
    {
        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public byte[] RowVersion { get; set; }

        [NotMapped]
        public ObjectState ObjectState { get; set; }

        /// <summary>
        /// Returns an equivalent EntityState for this entity's current local
        /// ObjectState. If the current ObjectState cannot be determined, the
        /// entity is considered Unchanged.
        /// </summary>
        public EntityState GetEntityState()
        {
            var result = EntityState.Unchanged;

            switch (ObjectState)
            {
                case ObjectState.Added:
                    result = EntityState.Added;
                    break;

                case ObjectState.Modified:
                    result = EntityState.Modified;
                    break;

                case ObjectState.Deleted:
                    result = EntityState.Deleted;
                    break;
            }

            return result;
        }
    }
}
