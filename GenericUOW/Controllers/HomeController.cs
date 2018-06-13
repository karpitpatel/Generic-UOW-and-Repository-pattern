using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericUOW.Domain.Manager;

namespace GenericUOW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestUOWManager _testUOWManager;
        public HomeController(ITestUOWManager testUOWManager)
        {
            _testUOWManager = testUOWManager;
        }

        public ActionResult Index()
        {
            var viewModelList = _testUOWManager.GetAllTestUOWs();
            var viewModel = _testUOWManager.GetTestUOW(1);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}