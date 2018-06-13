using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericUOW.Core.Data.Model
{
    /// <summary>
    /// An enumeration that represents the various persistence states of an
    /// IStateEntity. These values represent locally-tracked states, and
    /// correspond to an equivalent EntityState. The single exception is the
    /// (default) Unknown state, which can be used by entities that have no
    /// need of local state tracking. State synchronisation will therefore
    /// be only performed for entites that explicitly set their local state.
    /// </summary>
    public enum ObjectState
    {
        Unknown,
        Unchanged,
        Added,
        Modified,
        Deleted
    }

    /// <summary>
    /// Represents an entity that supports local state tracking, which
    /// helps to support persistence between disconntected contexts.
    /// </summary>
    public interface IStateEntity
    {
        ObjectState ObjectState { get; set; }

        EntityState GetEntityState();
    }
}
