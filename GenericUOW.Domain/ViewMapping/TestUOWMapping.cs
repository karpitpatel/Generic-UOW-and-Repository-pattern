using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericUOW.Data.Model;
using GenericUOW.Domain.ViewModel;

namespace GenericUOW.Domain.ViewMapping
{

    public class TestUOWMapping : BaseMapper<TestUOWViewModel, TestUOW>
    {
        public TestUOWMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Configure AutoMapper
                cfg.CreateMap<TestUOWViewModel, TestUOW>();
                cfg.CreateMap<TestUOW, TestUOWViewModel>();
            });
            Mapper = config.CreateMapper();
        }

    }
}
