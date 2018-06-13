using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Context;
using GenericUOW.Data.Model;

namespace GenericUOW.Data.Context
{
   public interface ITestUOWEntities:IDbContext
    {
        IDbSet<TestUOW> TestUOW { get; set; }
    }
}
