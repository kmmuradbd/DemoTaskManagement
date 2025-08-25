using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Infrastructure.Context
{
    public class TMPrincipal : IPrincipal
    {
        public TMPrincipal(TMIdentity identity)
        {
            this.Identity = identity;
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string roleId)
        {
            return ((TMIdentity)this.Identity).PermittedRoles.Any(x => x.Contains(roleId));
        }
    }
}
