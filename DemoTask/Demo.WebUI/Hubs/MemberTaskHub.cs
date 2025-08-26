using Demo.WebUI.DBQuery;
using Demo.WebUI.Helpers;
using DemoTask.Domain.DomainObject;
using Microsoft.AspNetCore.SignalR;

namespace Demo.WebUI.Hubs
{
    public class MemberTaskHub:Hub
    {
        MemberTaskDBQuery AppMemberTask;
        UserDBQuery AppUser;

        public MemberTaskHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TMConnection");
            AppMemberTask = new MemberTaskDBQuery(connectionString);
            AppUser = new UserDBQuery(connectionString);
        }

        public async Task SendMemberTasks()
        {
            var memberTasks = AppMemberTask.GetMemberTasks();
            await Clients.All.SendAsync("ReceivedMemberTasks", memberTasks);
        }

        public async Task SendUser()
        {
            var httpContext = Context.GetHttpContext();
          string userName=  SessionHelper.GetObjectFromJson<string>(httpContext.Session, "userName");
            var userData = AppUser.GetUser(userName);
            await Clients.All.SendAsync("ReceivedUser", userData);
        }

    }
}
