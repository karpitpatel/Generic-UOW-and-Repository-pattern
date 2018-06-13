using GenericUOW.Data.Context;
using GenericUOW.Data.Repository;
using GenericUOW.Domain.Manager;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenericUOW.Injection
{
    public class NinjectResolver : System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            _kernel.Bind<ITestUOWEntities>().To<TestUOWEntities>();
            _kernel.Bind<ITestUOWUnitOfWork>().To<TestUOWUnitOfWork>();
            _kernel.Bind<ITestUOWManager>().To<TestUOWManager>();
            _kernel.Bind<ITestUOWRepository>().To<TestUOWRepository>();
        }
    }
}