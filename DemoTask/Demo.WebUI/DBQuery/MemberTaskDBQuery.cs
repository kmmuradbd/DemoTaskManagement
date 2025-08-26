using Demo.WebUI.Models;
using System.Data;
using System.Data.SqlClient;

namespace Demo.WebUI.DBQuery
{
    public class MemberTaskDBQuery
    {
        string connectionString;

        public MemberTaskDBQuery(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<MemberTasks> GetMemberTasks()
        {
            List<MemberTasks> memberTasks = new List<MemberTasks>();
            MemberTasks memberTask;

            var data = GetMemberTaskFromDb();

            foreach (DataRow row in data.Rows)
            {
                memberTask = new MemberTasks
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                  //  StartDate = Convert.ToDateTime(row["StartDate"].ToString()),
                   // EndDate = Convert.ToDateTime(row["EndDate"].ToString()),
                   // ManagerName = row["ManagerName"].ToString(),
                    Remarks = row["Remarks"].ToString(),
                };
                memberTasks.Add(memberTask);
            }

            return memberTasks;
        }
        private DataTable GetMemberTaskFromDb()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Use the stored procedure name instead of SQL query
                    using (SqlCommand command = new SqlCommand("Sp_GetMemberTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Example: add parameters if the SP requires them
                        // command.Parameters.AddWithValue("@ManagerId", managerId);

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
