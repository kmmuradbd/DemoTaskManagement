using Demo.WebUI.Helpers;
using DemoTask.Domain.DomainObject;
using DemoTask.Service.Interface;
using DemoTask.Service.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.WebUI.Controllers
{
    public class ProjectController : Controller
    {
        protected readonly IProjectService AppProject;
        public ProjectController(IProjectService project)
        {
            this.AppProject = project;
        }
        // GET: Project
        public ActionResult Index()
        {
            try
            {
                IEnumerable<ProjectViewModel> projects = null;
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
                string roleId = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "roleId");
                if (roleId == "2")
                {
                    projects = AppProject.GetAll(userName);

                }
                else
                    projects = AppProject.GetAll();

                return View(projects);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region ddl
        public JsonResult GetProjectList()
        {
            string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
            string roleId = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "roleId");
            IEnumerable<object> projectList = null;
            if (roleId == "2")
            {
                 projectList = AppProject.GetProjectList(userName);
            }
            else
                projectList = AppProject.GetProjectList();

            return Json(projectList);
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProjectViewModel project)
        {
            try
            {
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
                project.CreatedDate = DateTime.Now;
                project.CreatedBy = userName;
                AppProject.Add(project);

                return Json(new
                {
                    message = "Data saved successfully.",
                    status = "success",
                    redirectUrl = "/Project"
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
            var user = AppProject.Get(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(ProjectViewModel project)
        {
            try
            {
                string userName = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "userName");
                project.UpdatedDate = DateTime.Now;
                project.UpdatedBy = userName;
                AppProject.Update(project);

                return Json(new
                {
                    message = "Data saved successfully.",
                    status = "success",
                    redirectUrl = "/Project"
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
