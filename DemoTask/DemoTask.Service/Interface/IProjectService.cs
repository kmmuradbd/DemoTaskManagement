using DemoTask.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.Interface
{
    public interface IProjectService
    {
        void Add(ProjectViewModel project);
        void Update(ProjectViewModel project);
        ProjectViewModel Get(int id);
        IEnumerable<ProjectViewModel> GetAll();
        IEnumerable<ProjectViewModel> GetAll(string managerId);
        IEnumerable<object> GetProjectList();
        IEnumerable<object> GetProjectList(string managerId);
    }
}
