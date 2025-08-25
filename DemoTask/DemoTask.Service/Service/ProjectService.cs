using DemoTask.Domain.RepositoryContract;
using DemoTask.Infrastructure.Context;
using DemoTask.Service.Interface;
using DemoTask.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.Service
{
    public class ProjectService : IProjectService
    {
        protected readonly IProjectRepository RepoProject;
        protected readonly IUserRepository RepoUser;
        public ProjectService(IProjectRepository project, IUserRepository repoUser)
        {
            this.RepoProject = project;
            RepoUser = repoUser;
        }
        public void Add(ProjectViewModel project)
        {
            try
            {
                project.Id = RepoProject.GetAutoNumber();
                project.CreatedDate = DateTime.Now;
                RepoProject.Add(project.ToEntity());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("IX_ProjectName"))
                {
                    throw new Exception("This Name(" + project.Name + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void Update(ProjectViewModel project)
        {
            try
            {
                ProjectViewModel oldProject = Get(project.Id);
                oldProject.IsActive = project.IsActive;
                oldProject.Name = project.Name;
                oldProject.StartDate = project.StartDate;
                oldProject.EndDate = project.EndDate;
                oldProject.ManagerId = project.ManagerId;
                oldProject.Remarks = project.Remarks;
                oldProject.UpdatedBy = project.UpdatedBy;
                oldProject.UpdatedDate = DateTime.Now;
                RepoProject.Update(oldProject.ToEntity());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("IX_ProjectName"))
                {
                    throw new Exception("This Name(" + project.Name + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public ProjectViewModel Get(int id)
        {
            var project = RepoProject.Get(r => r.Id == id && !r.IsArchived);
            return new ProjectViewModel
            {
                Id = project.Id,
                IsActive = project.IsActive,
                Name = project.Name,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ManagerId = project.ManagerId,
                Remarks = project.Remarks,
                IsArchived = project.IsArchived,
                CreatedBy = project.CreatedBy,
                CreatedDate = project.CreatedDate,
                UpdatedBy = project.UpdatedBy,
                UpdatedDate = project.UpdatedDate,
                //
            };
        }

        public IEnumerable<ProjectViewModel> GetAll()
        {

            try
            {
                var projectList = (from project in RepoProject.GetAll(r => !r.IsArchived)
                                   orderby project.Name
                                   select new ProjectViewModel()
                                   {
                                       Id = project.Id,
                                       Name = project.Name,
                                       IsActive = project.IsActive,
                                       StartDate = project.StartDate,
                                       EndDate = project.EndDate,
                                       ManagerId = project.ManagerId,
                                       Remarks = project.Remarks,
                                       ManagerName = RepoUser.Get(r => r.UserName == project.ManagerId).FullName,
                                   }).ToList();
                return projectList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IEnumerable<ProjectViewModel> GetAll(string managerId)
        {
            var projectList = (from project in RepoProject.GetAll(r => (r.ManagerId == managerId && !r.IsArchived))
                               orderby project.Name
                               select new ProjectViewModel()
                               {
                                   Id = project.Id,
                                   IsActive = project.IsActive,
                                   Name = project.Name,
                                   StartDate = project.StartDate,
                                   EndDate = project.EndDate,
                                   ManagerId = project.ManagerId,
                                   ManagerName = RepoUser.Get(r => r.UserName == project.ManagerId).FullName,
                                   Remarks = project.Remarks,
                                   IsArchived = project.IsArchived,
                                   CreatedBy = project.CreatedBy,
                                   CreatedDate = project.CreatedDate,
                                   UpdatedBy = project.UpdatedBy,
                                   UpdatedDate = project.UpdatedDate
                               }).ToList();
            return projectList;
        }

        public IEnumerable<object> GetProjectList()
        {
            return from r in RepoProject.GetAll(r => !r.IsArchived)
                   select new { Text = r.Name, Value = r.Id };
        }
        public IEnumerable<object> GetProjectList(string managerId)
        {
            return from r in RepoProject.GetAll(r => r.ManagerId == managerId && !r.IsArchived)
                   select new { Text = r.Name, Value = r.Id };
        }
    }
}
