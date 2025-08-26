using Demo.WebUI.Models;
using System.Data;
using System.Data.SqlClient;

namespace Demo.WebUI.DBQuery
{
    public class UserDBQuery
    {
        string connectionString;

        public UserDBQuery(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Users GetUser(string userName)
        {
            int? roleId = GetUserRoleFromDb(userName); // now returns int?

            if (roleId.HasValue)
            {
                return new Users
                {
                    UserRoleMasterId = roleId.Value
                };
            }

            return null; // or handle as needed
        }

        private int? GetUserRoleFromDb(string userName)
        {

            var query = "SELECT UserRoleMasterId FROM [dbo].[Users] WHERE IsActive = 1 AND UserName = '"+ userName + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.Text; // ✅ raw SQL

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            return null; // no rows found
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

    }
}
