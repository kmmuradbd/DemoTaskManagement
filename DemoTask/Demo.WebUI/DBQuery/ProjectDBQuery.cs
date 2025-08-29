using Demo.WebUI.Models;
using System.Data;
using System.Data.SqlClient;

namespace Demo.WebUI.DBQuery
{
    public class ProjectDBQuery
    {
        string connectionString;

        public ProjectDBQuery(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Projects> GetProjects(string memberId, string roleId)
        {
            List<Projects> projects = new List<Projects>();
            Projects project;

            var data = GetProjectDetailsFromDb(memberId, roleId);

            foreach (DataRow row in data.Rows)
            {
                project = new Projects
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    StartDate =Convert.ToDateTime( row["StartDate"].ToString()),
                    EndDate =Convert.ToDateTime( row["EndDate"].ToString()),
                    ManagerName = row["ManagerName"].ToString(),
                    Remarks = row["Remarks"].ToString(),
                };
                projects.Add(project);
            }

            return projects;
        }
        private DataTable GetProjectDetailsFromDb(string memberId, string roleId)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use the stored procedure name instead of SQL query
                    using (SqlCommand command = new SqlCommand("Sp_GetProject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserName", memberId);
                        command.Parameters.AddWithValue("@RoleId", roleId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }

                    return dataTable;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }
}
