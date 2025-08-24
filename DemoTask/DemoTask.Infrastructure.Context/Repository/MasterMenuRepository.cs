using DemoTask.Domain.DomainObject;
using DemoTask.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Infrastructure.Context.Repository
{
    public class MasterMenuRepository : Repository<MasterMenu>, IMasterMenuRepository
    {
        public MasterMenuRepository(DemoTaskContext dbContext)
     : base(dbContext)
        {

        }
    }
}
