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
        Task<IEnumerable<UserViewModel>> GetAll();
        Task<IEnumerable<object>> GetUserList();
        UserViewModel? Login(string userName, string password);
    }
}
