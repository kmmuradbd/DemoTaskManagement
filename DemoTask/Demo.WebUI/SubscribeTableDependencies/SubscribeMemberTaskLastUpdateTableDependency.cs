using Demo.WebUI.Hubs;
using Demo.WebUI.Models;
using DemoTask.Domain.DomainObject;
using TableDependency.SqlClient;

namespace Demo.WebUI.SubscribeTableDependencies
{
    public class SubscribeMemberTaskLastUpdateTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<MemberTasks> tableDependency;
        MemberTaskHub memberTaskHub;

        public SubscribeMemberTaskLastUpdateTableDependency(MemberTaskHub memberTask)
        {
            this.memberTaskHub = memberTask;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<MemberTasks>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<MemberTasks> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                memberTaskHub.SendMemberTaskLastUpdate();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Project)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}
