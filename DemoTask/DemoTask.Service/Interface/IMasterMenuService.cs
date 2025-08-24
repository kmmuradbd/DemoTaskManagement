using DemoTask.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.Interface
{
    public interface IMasterMenuService
    {
        void Add(MasterMenuViewModel masterMenu);
        void Update(MasterMenuViewModel masterMenu);
        MasterMenuViewModel Get(string id);
        List<MasterMenuViewModel> GetAll();
        List<MasterMenuViewModel> GetAll(string userId);
    }
}
