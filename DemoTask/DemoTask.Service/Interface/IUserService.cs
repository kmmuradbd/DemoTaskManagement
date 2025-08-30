using DemoTask.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.Interface
{
    public interface IUserService
    {
        void Add(UserViewModel user);
        void Update(UserViewModel user);
        UserViewModel Get(int id);
        UserViewModel Get(string memberId);
        Task<IEnumerable<UserViewModel>> GetAll();
        UserViewModel? Login(string userName, string password);
        IEnumerable<object> GetUserRoleMasterList();
        IEnumerable<object> GetUserList(int roleId);

    }
}
