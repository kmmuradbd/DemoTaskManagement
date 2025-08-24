using Demo.WebUI.Helpers;
using Demo.WebUI.Models;
using DemoTask.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demo.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected readonly IMasterMenuService AppMasterMenu;
        public HomeController(ILogger<HomeController> logger, IMasterMenuService masterMenu)
        {
            _logger = logger;
            this.AppMasterMenu = masterMenu;
        }

        public IActionResult Index()
        {
            try
            {
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");


                if (string.IsNullOrEmpty(userName))
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    var data = AppMasterMenu.GetAll(userName);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "menu", data);
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
