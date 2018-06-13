using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUOW.Core.Data.Repository;
using GenericUOW.Data.Model;

namespace GenericUOW.Data.Repository
{
    public interface ITestUOWRepository : IRepository<TestUOW>
    {
        TestUOW GetTestUOW(int id);
        ICollection<TestUOW> GetTestUOWs();
    }
    public class TestUOWRepository : BaseRepository<TestUOW>, ITestUOWRepository
    {
        public TestUOWRepository(ITestUOWUnitOfWork unitOfWork)
            :base(unitOfWork)
        {
                
        }
        public TestUOW GetTestUOW(int id)
        {
            return Get(_ => _.Id == id).FirstOrDefault();
        }
        public ICollection<TestUOW> GetTestUOWs()
        {
            return Get().ToList();
        }
    }
}
