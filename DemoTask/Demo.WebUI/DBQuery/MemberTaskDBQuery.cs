using Demo.WebUI.Helpers;
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

        public List<MemberTasks> GetMemberTasks(string memberId, string roleId)
        {
            List<MemberTasks> memberTasks = new List<MemberTasks>();
            MemberTasks memberTask;

            var data = GetMemberTaskFromDb(memberId, roleId);

            foreach (DataRow row in data.Rows)
            {
                memberTask = new MemberTasks
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Remarks = row["Remarks"].ToString(),
                };
                memberTasks.Add(memberTask);
            }

            return memberTasks;
        }
        private DataTable GetMemberTaskFromDb(string memberId, string roleId)
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

        public List<MemberTasks> GetMemberTaskLastUpdates(string memberId, string createdDate)
        {
            List<MemberTasks> memberTasks = new List<MemberTasks>();
            MemberTasks memberTask;

            var data = GetMemberTaskLastUpdateFromDb(memberId, createdDate);

            foreach (DataRow row in data.Rows)
            {
                memberTask = new MemberTasks
                {
                    Name = row["Name"].ToString(),
                };
                memberTasks.Add(memberTask);
            }

            return memberTasks;
        }
        private DataTable GetMemberTaskLastUpdateFromDb(string memberId, string createdDate)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sqlCommand = @"SELECT [Name] FROM [dbo].[MemberTasks] WHERE  MemberId =@MemberId and [CreatedDate] > @CreatedDate";
                    // Use the stored procedure name instead of SQL query
                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Example: add parameters if the SP requires them
                        command.Parameters.AddWithValue("@UserName", memberId);
                        command.Parameters.AddWithValue("@CreatedDate", createdDate);

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
