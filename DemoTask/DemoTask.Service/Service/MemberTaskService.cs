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
    public class MemberTaskService : IMemberTaskService
    {
        protected readonly IMemberTaskRepository RepoMemberTask;
        protected readonly IUserRepository RepoUser;
        protected readonly IProjectRepository RepoProject;
        public MemberTaskService(IMemberTaskRepository memberTask, IUserRepository repoUser, IProjectRepository repoProject)
        {
            this.RepoMemberTask = memberTask;
            RepoUser = repoUser;
            RepoProject = repoProject;
        }
        public void Add(MemberTaskViewModel memberTask)
        {
            try
            {
                memberTask.Id = RepoMemberTask.GetAutoNumber();
                memberTask.Status = "Pending";
                RepoMemberTask.Add(memberTask.ToEntity());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("IX_MemberTaskName"))
                {
                    throw new Exception("This Name(" + memberTask.Name + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void Update(MemberTaskViewModel memberTask)
        {
            try
            {
                MemberTaskViewModel oldMemberTask = Get(memberTask.Id);
                oldMemberTask.IsActive = memberTask.IsActive;
                oldMemberTask.Name = memberTask.Name;
                oldMemberTask.ProjectId = memberTask.ProjectId;
                oldMemberTask.MemberId = memberTask.MemberId;
                oldMemberTask.Status = memberTask.Status;
                oldMemberTask.Remarks = memberTask.Remarks;
                oldMemberTask.UpdatedBy = memberTask.UpdatedBy;
                oldMemberTask.UpdatedDate = DateTime.Now;
                RepoMemberTask.Update(oldMemberTask.ToEntity());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("IX_MemberTaskName"))
                {
                    throw new Exception("This Name(" + memberTask.Name + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public MemberTaskViewModel Get(int id)
        {
            var memberTask = RepoMemberTask.Get(r => r.Id == id && !r.IsArchived);
            return new MemberTaskViewModel
            {
                Id = memberTask.Id,
                IsActive = memberTask.IsActive,
                Name = memberTask.Name,
                ProjectId = memberTask.ProjectId,
                MemberId = memberTask.MemberId,
                Status = memberTask.Status,
                Remarks = memberTask.Remarks,
                IsArchived = memberTask.IsArchived,
                CreatedBy = memberTask.CreatedBy,
                CreatedDate = memberTask.CreatedDate,
                UpdatedBy = memberTask.UpdatedBy,
                UpdatedDate = memberTask.UpdatedDate,
            };
        }

        public IEnumerable<MemberTaskViewModel> GetAll()
        {
            var memberTaskList = (from memberTask in RepoMemberTask.GetAll(r => !r.IsArchived)
                                  orderby memberTask.Name
                                  select new MemberTaskViewModel()
                                  {
                                      Id = memberTask.Id,
                                      IsActive = memberTask.IsActive,
                                      Name = memberTask.Name,
                                      ProjectId = memberTask.ProjectId,
                                      ProjectName = RepoProject.Get(memberTask.ProjectId).Name,
                                      MemberId = memberTask.MemberId,
                                      MemberName = RepoUser.Get(r => r.UserName == memberTask.MemberId).FullName,
                                      Status = memberTask.Status,
                                      Remarks = memberTask.Remarks,
                                      IsArchived = memberTask.IsArchived,
                                      CreatedBy = memberTask.CreatedBy,
                                      CreatedDate = memberTask.CreatedDate,
                                      UpdatedBy = memberTask.UpdatedBy,
                                      UpdatedDate = memberTask.UpdatedDate,
                                  }).OrderByDescending(o => o.CreatedDate).ToList();
            return memberTaskList;
        }
        public IEnumerable<MemberTaskViewModel> GetAll(string memberId)
        {
            var memberTaskList = (from memberTask in RepoMemberTask.GetAll(r =>r.MemberId== memberId && !r.IsArchived)
                                  orderby memberTask.Name
                                  select new MemberTaskViewModel()
                                  {
                                      Id = memberTask.Id,
                                      IsActive = memberTask.IsActive,
                                      Name = memberTask.Name,
                                      ProjectId = memberTask.ProjectId,
                                      ProjectName = RepoProject.Get(memberTask.ProjectId).Name,
                                      MemberId = memberTask.MemberId,
                                      MemberName = RepoUser.Get(r => r.UserName == memberTask.MemberId).FullName,
                                      Status = memberTask.Status,
                                      Remarks = memberTask.Remarks,
                                      IsArchived = memberTask.IsArchived,
                                      CreatedBy = memberTask.CreatedBy,
                                      CreatedDate = memberTask.CreatedDate,
                                      UpdatedBy = memberTask.UpdatedBy,
                                      UpdatedDate = memberTask.UpdatedDate,
                                  }).OrderByDescending(o => o.CreatedDate).ToList();
            return memberTaskList;
        }
        public IEnumerable<MemberTaskViewModel> GetAllCretedBy(string createdBy)
        {
            var memberTaskList = (from memberTask in RepoMemberTask.GetAll(r => !r.IsArchived)
                                  join project in RepoProject.GetAll()
                                  on memberTask.ProjectId equals project.Id
                                  join member in RepoUser.GetAll()
                                  on memberTask.MemberId equals member.UserName into memberGroup
                                  from member in memberGroup.DefaultIfEmpty()
                                  where project.ManagerId == createdBy
                                  orderby memberTask.Name
                                  select new MemberTaskViewModel
                                  {
                                      Id = memberTask.Id,
                                      IsActive = memberTask.IsActive,
                                      Name = memberTask.Name,
                                      ProjectId = memberTask.ProjectId,
                                      ProjectName = project?.Name ?? "",
                                      MemberId = memberTask.MemberId,
                                      MemberName = member?.FullName ?? "",
                                      Status = memberTask.Status,
                                      Remarks = memberTask.Remarks,
                                      IsArchived = memberTask.IsArchived,
                                      CreatedBy = memberTask.CreatedBy,
                                      CreatedDate = memberTask.CreatedDate,
                                      UpdatedBy = memberTask.UpdatedBy,
                                      UpdatedDate = memberTask.UpdatedDate,
                                  })
                                  .OrderByDescending(o => o.CreatedDate)
                                  .ToList();

            return memberTaskList;
        }


        public IEnumerable<MemberTaskViewModel> GetAll(string memberId, DateTime createDate)
        {
            var memberTaskList = (from memberTask in RepoMemberTask.GetAll(r => r.MemberId == memberId && r.CreatedDate > createDate && !r.IsArchived)
                                  orderby memberTask.Name
                                  select new MemberTaskViewModel()
                                  {
                                      Id = memberTask.Id,
                                      IsActive = memberTask.IsActive,
                                      Name = memberTask.Name,
                                      Status = memberTask.Status,
                                  }).OrderByDescending(a => a.CreatedDate).ToList();
            return memberTaskList;
        }

    }
}
