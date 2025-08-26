using Demo.WebUI.Hubs;
using Demo.WebUI.Models;
using DemoTask.Domain.DomainObject;
using TableDependency.SqlClient;

namespace Demo.WebUI.SubscribeTableDependencies
{
    public class SubscribeUserTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Users> tableDependency;
        MemberTaskHub projectHub;

        public SubscribeUserTableDependency(MemberTaskHub projectHub)
        {
            this.projectHub = projectHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Users>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Users> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                projectHub.SendUser();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Users)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}
