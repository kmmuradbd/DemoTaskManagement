using Demo.WebUI.DBQuery;
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
            var projects = AppProject.GetProjects();
            await Clients.All.SendAsync("ReceivedProjects", projects);

        }

    }
}
