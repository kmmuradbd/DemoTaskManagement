using Demo.WebUI.Helpers;
using DemoTask.Service.Interface;
using DemoTask.Service.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Demo.WebUI.Controllers
{
    public class UserController : Controller
    {
        protected readonly IUserService AppUser;
        public UserController(IUserService userService)
        {
            this.AppUser = userService;
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                IEnumerable<UserViewModel> users = await AppUser.GetAll();
                return View(users);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region ddl
        [HttpGet("GetUserRoleMasterList")]
        public JsonResult GetUserRoleMasterList()
        {
            var userList = AppUser.GetUserRoleMasterList();
            return Json(userList);
        }
        public JsonResult GetUserList(int roleId)
        {
            var userList = AppUser.GetUserList(roleId);
            return Json(userList);
        }
        #endregion

        #region Create
        [HttpGet("Create")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public IActionResult Create(UserViewModel user)
        {
            try
            {
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = userName;
                AppUser.Add(user);

                return Json(new
                {
                    message = "Data saved successfully.",
                    status = "success",
                    redirectUrl = "/User"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = ex.Message,
                    status = "failure"
                });
            }
        }


        #endregion

        #region Update
        [HttpGet("Edit")]
        public IActionResult Edit(int id)
        {
            var user = AppUser.Get(id);
            return View(user);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(UserViewModel user)
        {
            try
            {
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
                user.UpdatedDate = DateTime.Now;
                user.UpdatedBy = userName;
                AppUser.Update(user);

                return Json(new
                {
                    message = "Data saved successfully.",
                    status = "success",
                    redirectUrl = "/User"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    message = ex.Message,
                    status = "failure"
                });
            }
        }
        #endregion
    }
}
