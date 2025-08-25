using Demo.WebUI.Hubs;
using Demo.WebUI.Models;
using DemoTask.Domain.DomainObject;
using TableDependency.SqlClient;

namespace Demo.WebUI.SubscribeTableDependencies
{
    public class SubscribeProjectTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<Projects> tableDependency;
        ProjectHub projectHub;

        public SubscribeProjectTableDependency(ProjectHub projectHub)
        {
            this.projectHub = projectHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Projects>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Projects> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                projectHub.SendProjects();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Project)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}
