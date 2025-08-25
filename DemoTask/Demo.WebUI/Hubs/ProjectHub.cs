using Demo.WebUI.DBQuery;
using Microsoft.AspNetCore.SignalR;

namespace Demo.WebUI.Hubs
{
    public class ProjectHub:Hub
    {
        ProjectDBQuery AppProject;
       // SaleRepository saleRepository;
       // CustomerRepository customerRepository;

        public ProjectHub(IConfiguration configuration)
        {
             var connectionString = configuration.GetConnectionString("TMConnection");
            AppProject = new ProjectDBQuery(connectionString);
           // saleRepository = new SaleRepository(connectionString);
           // customerRepository = new CustomerRepository(connectionString);
        }

        public async Task SendProjects()
        {
            var projects = AppProject.GetProjects();
            await Clients.All.SendAsync("ReceivedProjects", projects);

          //  var productsForGraph = productRepository.GetProductsForGraph();
          //  await Clients.All.SendAsync("ReceivedProductsForGraph", productsForGraph);
        }

        //public async Task SendSales()
        //{
        //    var sales = saleRepository.GetSales();
        //    await Clients.All.SendAsync("ReceivedSales", sales);

        //    var salesForGraph = saleRepository.GetSalesForGraph();
        //    await Clients.All.SendAsync("ReceivedSalesForGraph", salesForGraph);
        //}

        //public async Task SendCustomers()
        //{
        //    var customers = customerRepository.GetCustomers();
        //    await Clients.All.SendAsync("ReceivedCustomers", customers);

        //    var customersForGraph = customerRepository.GetCustomersForGraph();
        //    await Clients.All.SendAsync("ReceivedCustomersForGraph", customersForGraph);
        //}
    }
}
