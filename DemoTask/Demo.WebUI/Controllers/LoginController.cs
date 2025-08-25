using Demo.WebUI.Helpers;
using DemoTask.Infrastructure.Context;
using DemoTask.Service.Interface;
using DemoTask.Service.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService userService;
        private readonly ILogger<HomeController> _logger;

        public LoginController(ILogger<HomeController> logger, IUserService user)
        {
            _logger = logger;
            this.userService = user;
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
                SessionHelper.SetObjectAsJson(HttpContext.Session, "isSysAdmin", isSysAdmin);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "roleId", isAuthenticated.UserRoleMasterId);
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                return Content(string.Format("UYResult('{0}','{1}')",
                     "UserName or Password Wrong", "failure"), "application/javascript");
            }
        }
    }
}
