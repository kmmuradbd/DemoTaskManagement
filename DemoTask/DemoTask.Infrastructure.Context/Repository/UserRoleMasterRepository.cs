using DemoTask.Domain.DomainObject;
using DemoTask.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Infrastructure.Context.Repository
{
    public class UserRoleMasterRepository:Repository<UserRoleMaster>, IUserRoleMasterRepository
    {
        public UserRoleMasterRepository(DemoTaskContext dbContext)
   : base(dbContext)
        {

        }
    }
}
