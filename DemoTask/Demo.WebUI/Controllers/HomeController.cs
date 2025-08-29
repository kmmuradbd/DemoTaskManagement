using Demo.WebUI.CustomMiddleware;
using Demo.WebUI.Helpers;
using Demo.WebUI.Models;
using DemoTask.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

namespace Demo.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected readonly IMasterMenuService AppMasterMenu;
        protected readonly IMemberTaskService AppMemberTask;
        private readonly OnlineUsersService _onlineUsers;
        public HomeController(ILogger<HomeController> logger, IMasterMenuService masterMenu, IMemberTaskService appMemberTask, OnlineUsersService onlineUsersService)
        {
            _logger = logger;
            this.AppMasterMenu = masterMenu;
            AppMemberTask = appMemberTask;
            _onlineUsers = onlineUsersService;
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
                    var users = _onlineUsers.GetUsers();
                    return View(users);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult GetNotificationContacts()
        {
           // string created = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "oldcreatedDate");
            string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
            // DateTime createdDate = DateTime.Parse(created);
            DateTime lastCreatedDate = NotificationCache.GetLastCreatedDate(userName) ?? DateTime.MinValue;

            var Data = AppMemberTask.GetAll(userName, lastCreatedDate);

            return Json(Data);
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
