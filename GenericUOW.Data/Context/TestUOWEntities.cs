using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Context;
using GenericUOW.Data.Mapping;
using GenericUOW.Data.Model;

namespace GenericUOW.Data.Context
{
   public class TestUOWEntities : BaseDbContext, ITestUOWEntities
    {
        public TestUOWEntities()
        {
                
        }
        public TestUOWEntities(string connectionString)
            :base(connectionString)
        {
                
        }

        public IDbSet<TestUOW> TestUOW { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null) return;
            
            modelBuilder.Configurations.Add(new TestUOWMapping());
            
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
