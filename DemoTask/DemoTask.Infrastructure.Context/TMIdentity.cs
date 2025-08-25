using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Infrastructure.Context
{
    public class TMIdentity : IIdentity
    {
        public TMIdentity(string basicTicket)
        {
            string[] ticketData = basicTicket.Split(new string[] { "__#" }, StringSplitOptions.None);
            this.Id = ticketData[0];
            this.Name = ticketData[1];
            this.IsAuthenticated = true;
            this.IsSysAdmin = Convert.ToBoolean(ticketData[2]);
            this.RoleId = string.IsNullOrEmpty(ticketData[3]?.ToString()) ? 0 : Convert.ToInt32(ticketData[3]);

        }
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string AuthenticationType { get { return "TMAuthentication"; } }
        public bool IsAuthenticated { get; private set; }
        public bool IsSysAdmin { get; set; }
        public int RoleId { get; set; }
        public string[] PermittedRoles { get; private set; }

        public static string CreateBasicTicket(
                                           string id,
                                           string name,
                                            bool isSysAdmin,
                                            int roleId
                                           )
        {
            return id + "__#"
                + name + "__#"
                + isSysAdmin + "__#"
                + roleId + "__#";
        }
        public static string CreateRoleTicket(string[] roles)
        {
            string rolesString = "";
            for (int i = 0; i < roles.Length; i++)
            {
                rolesString += roles[i] + ",";
            }
            rolesString.TrimEnd(new char[] { ',' });
            return rolesString + "__#";
        }

        public void SetRoles(string roleTicket)
        {
            this.PermittedRoles = roleTicket == "" ? new string[0] : roleTicket.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
