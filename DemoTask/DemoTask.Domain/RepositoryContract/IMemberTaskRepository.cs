using DemoTask.Core;
using DemoTask.Domain.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Domain.RepositoryContract
{
    public interface IMemberTaskRepository : IRepository<MemberTask>
    {
    }
}
