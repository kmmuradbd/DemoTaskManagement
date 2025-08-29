using Demo.WebUI.DBQuery;
using Demo.WebUI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Demo.WebUI.Hubs
{
    public class ProjectHub:Hub
    {
        ProjectDBQuery AppProject;

        public ProjectHub(IConfiguration configuration)
        {
             var connectionString = configuration.GetConnectionString("TMConnection");
            AppProject = new ProjectDBQuery(connectionString);
        }

        public async Task SendProjects()
        {
            var httpContext = Context.GetHttpContext();
            string userName = SessionHelper.GetObjectFromJson<string>(httpContext.Session, "userName");
            string roleId = SessionHelper.GetObjectFromJson<string>(httpContext.Session, "roleId");
            var projects = AppProject.GetProjects(userName, roleId);
            await Clients.All.SendAsync("ReceivedProjects", projects);

        }

    }
}
