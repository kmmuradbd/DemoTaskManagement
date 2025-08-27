using DemoTask.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.Interface
{
    public interface IMemberTaskService
    {
        void Add(MemberTaskViewModel memberTask);
        void Update(MemberTaskViewModel memberTask);
        MemberTaskViewModel Get(int id);
        IEnumerable<MemberTaskViewModel> GetAll();
        IEnumerable<MemberTaskViewModel> GetAll(string memberId);
        IEnumerable<MemberTaskViewModel> GetAll(string memberId, DateTime createDate);
        IEnumerable<MemberTaskViewModel> GetAllCretedBy(string createdBy);
    }
}
