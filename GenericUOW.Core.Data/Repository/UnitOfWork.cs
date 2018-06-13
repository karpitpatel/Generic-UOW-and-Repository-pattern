using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Context;

namespace GenericUOW.Core.Data.Repository
{
    /// <summary>
    /// A base implementation of UnitOfWork, tied to a particular context type.
    /// </summary>
    /// <typeparam name="TContext">the type of context to base this UOW on</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : IDbContext
    {
        // A flag that ensures that this object is disposed of once only, since
        // we are handling disposing locally.
        private bool disposed = false;

        /// <summary>
        /// Creates a new UnitOfWork around the given context object.
        /// </summary>
        /// <param name="context">the context that this UOW applies to</param>
        public UnitOfWork(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// The context that this UOW is operating under.
        /// </summary>
        public IDbContext Context { get; private set; }

        /// <summary>
        /// Invokes a save on the associated context.
        /// </summary>
        public int Save()
        {
            return Context.SaveChanges();
        }

        /// <summary>
        /// Disposes of this UOW and its underlying context, according to IDispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implements IDisposable for this object. The boolean value provided
        /// is used to identify calls by the Finalizer (disposing=false),
        /// separating the disposal of managed & unmanaged resources.
        /// </summary>
        /// <param name="disposing">true for a call from Dispose(); false from the Finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (!disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                    Context = null;
                }
            }

            disposed = true;
        }
    }
}
