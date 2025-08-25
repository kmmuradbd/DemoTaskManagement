using Demo.WebUI.Helpers;
using DemoTask.Domain.DomainObject;
using DemoTask.Service.Interface;
using DemoTask.Service.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebUI.Controllers
{
    public class MemberTaskController : Controller
    {
        protected readonly IMemberTaskService AppMemberTask;
        public MemberTaskController(IMemberTaskService memberTask)
        {
            this.AppMemberTask = memberTask;
        }
        public ActionResult Index()
        {
            try
            {
                IEnumerable<MemberTaskViewModel> memberTasks = null;
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
                string roleId= SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "roleId");
                if(roleId=="3")
                {
                   memberTasks = AppMemberTask.GetAll(userName);

                }
                if (roleId == "2")
                {
                    memberTasks = AppMemberTask.GetAllCretedBy(userName);

                }
                else
                    memberTasks = AppMemberTask.GetAll();
                return View(memberTasks);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MemberTaskViewModel memberTask)
        {
            try
            {
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
                memberTask.CreatedDate = DateTime.Now;
                memberTask.CreatedBy = userName;
                AppMemberTask.Add(memberTask);

                return Json(new
                {
                    message = "Data saved successfully.",
                    status = "success",
                    redirectUrl = "/MemberTask"
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
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var task = AppMemberTask.Get(id);
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(MemberTaskViewModel memberTask)
        {
            try
            {
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
                memberTask.UpdatedDate = DateTime.Now;
                memberTask.UpdatedBy = userName;
               
                AppMemberTask.Update(memberTask);

                return Json(new
                {
                    message = "Data saved successfully.",
                    status = "success",
                    redirectUrl = "/MemberTask"
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
