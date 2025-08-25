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

        public List<Projects> GetProjects()
        {
            List<Projects> projects = new List<Projects>();
            Projects project;

            var data = GetProjectDetailsFromDb();

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
        private DataTable GetProjectDetailsFromDb()
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
                    // Optional: log exception
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        //private DataTable GetProjectDetailsFromDb()
        //{
        //    var query = "SELECT Id, Name FROM Projects";
        //    DataTable dataTable = new DataTable();

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    dataTable.Load(reader);
        //                }
        //            }

        //            return dataTable;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }
        //    }
        //}

        //public List<ProductForGraph> GetProductsForGraph()
        //{
        //    List<ProductForGraph> productsForGraph = new List<ProductForGraph>();
        //    ProductForGraph productForGraph;

        //    var data = GetProductsForGraphFromDb();

        //    foreach (DataRow row in data.Rows)
        //    {
        //        productForGraph = new ProductForGraph
        //        {
        //            Category = row["Category"].ToString(),
        //            Products = Convert.ToInt32(row["Products"])
        //        };
        //        productsForGraph.Add(productForGraph);
        //    }

        //    return productsForGraph;
        //}

        //private DataTable GetProductsForGraphFromDb()
        //{
        //    var query = "SELECT Category, COUNT(Id) Products FROM Product GROUP BY Category";
        //    DataTable dataTable = new DataTable();

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    dataTable.Load(reader);
        //                }
        //            }

        //            return dataTable;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }
        //    }
        //}
    }
}
