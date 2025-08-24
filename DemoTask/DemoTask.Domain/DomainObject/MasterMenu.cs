using DemoTask.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Domain.DomainObject
{
    public class MasterMenu : Entity
    {
        public string Name { get; set; }
        public long ParentId { get; set; }
        public string URL { get; set; }
        public int SortNo { get; set; }
        public string Icon { get; set; }
    }
}
