using Demo.WebUI.CustomMiddleware;
using Demo.WebUI.Helpers;
using DemoTask.Domain.DomainObject;
using DemoTask.Infrastructure.Context;
using DemoTask.Service.Interface;
using DemoTask.Service.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService userService;
        private readonly ILogger<HomeController> _logger;
        private readonly OnlineUsersService _userCache;

        public LoginController(ILogger<HomeController> logger, IUserService user, OnlineUsersService onlineUsers)
        {
            _logger = logger;
            this.userService = user;
            _userCache = onlineUsers;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(UserViewModel user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
            {
                return Content(string.Format("UYResult('{0}','{1}')",
                    "Please enter username or password", "failure"), "application/javascript");
            }

            var isAuthenticated = userService.Login(user.UserName.ToLower(), user.Password);

            if (isAuthenticated != null)
            {
                bool isSysAdmin = false;
                if (isAuthenticated.UserRoleMasterId == 1) isSysAdmin = true;
                string basicTicket = TMIdentity.CreateBasicTicket(
                                                                    isAuthenticated.UserName,
                                                                    isAuthenticated.FullName,
                                                                    isSysAdmin,
                                                                    isAuthenticated.UserRoleMasterId
                                                                 );
               // HttpContext.Application["BasicTicket" + username] = basicTicket;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "userName", user.UserName);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "userFullName", isAuthenticated.FullName);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "isSysAdmin", isSysAdmin);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "roleId", isAuthenticated.UserRoleMasterId);
                _userCache.AddUser(isAuthenticated.FullName);
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                return Content(string.Format("UYResult('{0}','{1}')",
                     "UserName or Password Wrong", "failure"), "application/javascript");
            }
        }

        #region Logout
        public ActionResult Logout()
        {
            string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userFullName");
            _userCache.RemoveUser(userName);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}
