using Dev.BO.Models;
using Dev.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDevTestServices _devTestService;

        public HomeController(IDevTestServices devTestService)
        {
            _devTestService = devTestService;
        }
        public ActionResult Index()
        {
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

        public ActionResult Add(int Id = 0)
        {
            var model = new DevTest();
            if (Id != 0)
            {
                model = _devTestService.GetDevTestDetailsById(Id);
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(DevTest model)
        {
            if (model.Id == 0)
                _devTestService.SaveDevTestDetails(model);
            else
                _devTestService.UpdateDevTestDetails(model);
            return RedirectToAction("List", "Home");
        }

        public ActionResult List()
        {
            List<DevTest> lstTestData = new List<DevTest>();
            lstTestData = _devTestService.GetDevTestDataList();
            return View(lstTestData);
        }
        public ActionResult Delete(int Id = 0)
        {
            if (Id != 0)
            {
                _devTestService.DeleteDevTestDetails(Id);
                return RedirectToAction("List", "Home");
            }
            return RedirectToAction("List", "Home");
        }
    }
}