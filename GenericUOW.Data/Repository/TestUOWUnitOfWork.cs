using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Repository;
using GenericUOW.Data.Context;

namespace GenericUOW.Data.Repository
{

    public interface ITestUOWUnitOfWork : IUnitOfWork
    {
    }
    /// <summary>
    /// An extension of UnitOfWork that specifically uses an instance of ITestUOWEntities,
    /// and forms the basis of all repositories that work with common data.
    /// </summary>
    public class TestUOWUnitOfWork : UnitOfWork<ITestUOWEntities>, ITestUOWUnitOfWork
    {
        public TestUOWUnitOfWork(ITestUOWEntities context)
            :base(context)
        {
                
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
