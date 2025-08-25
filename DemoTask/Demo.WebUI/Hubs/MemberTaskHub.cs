using Demo.WebUI.DBQuery;
using Microsoft.AspNetCore.SignalR;

namespace Demo.WebUI.Hubs
{
    public class MemberTaskHub:Hub
    {
        MemberTaskDBQuery AppMemberTask;

        public MemberTaskHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TMConnection");
            AppMemberTask = new MemberTaskDBQuery(connectionString);
        }

        public async Task SendMemberTasks()
        {
            var memberTasks = AppMemberTask.GetMemberTasks();
            await Clients.All.SendAsync("ReceivedMemberTasks", memberTasks);

        }
    }
}
