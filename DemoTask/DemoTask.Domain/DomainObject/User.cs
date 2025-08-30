using DemoTask.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Domain.DomainObject
{
    public class User : Entity
    {
       
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserRoleMasterId { get; set; }
        public string Email { get; set; }
    }
}
