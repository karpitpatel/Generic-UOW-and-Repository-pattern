using GenericUOW.Core.Data.Mapping;
using GenericUOW.Data.Mapping;
using GenericUOW.Data.Model;

namespace GenericUOW.Data.Mapping
{
    public class TestUOWMapping : BaseEntityMap<TestUOW>
    {
        public TestUOWMapping()
        {
            // Table and column mapping
            ToTable("TestUOW");

            // Primary key
            HasKey(t => t.Id);

        }
    }
}
