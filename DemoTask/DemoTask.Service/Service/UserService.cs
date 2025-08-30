using DemoTask.Domain.RepositoryContract;
using DemoTask.Infrastructure.Context;
using DemoTask.Service.Interface;
using DemoTask.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository RepoUser;
        protected readonly IUserRoleMasterRepository RepoUserRoleMaster;

        public UserService(IUserRepository user, IUserRoleMasterRepository repoUserRoleMaster)
        {
            RepoUser = user;
            RepoUserRoleMaster = repoUserRoleMaster;
        }

        public void Add(UserViewModel user)
        {
            try
            {
                user.Id = RepoUser.GetAutoNumber();
                user.Password= Common.HashCode(user.Password);
                RepoUser.Add(user.ToEntity());
            }
            catch (Exception ex)
            {

                if (ex.InnerException.InnerException.Message.Contains("PK_Users"))
                {
                    throw new Exception("This name(" + user.UserName + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }

            }
        }
        public void Update(UserViewModel user)
        {
            try
            {
                UserViewModel oldUser = Get(user.Id);

                oldUser.FullName = user.FullName;
                oldUser.Password =Common.HashCode(user.Password);
                oldUser.UserRoleMasterId = user.UserRoleMasterId;
                oldUser.Email= user.Email;
                oldUser.IsActive = user.IsActive;
                oldUser.UpdatedBy = user.UpdatedBy;
                oldUser.UpdatedDate = user.UpdatedDate;
                RepoUser.Update(oldUser.ToEntity());
            }
            catch (Exception ex)
            {

                if (ex.InnerException.InnerException.Message.Contains("IX_Users"))
                {
                    throw new Exception("This name(" + user.UserName + ") is already exists");
                }
                else
                {
                    throw new Exception(ex.Message);
                }

            }
        }

        public UserViewModel Get(int id)
        {
            var user = RepoUser.Get(r => r.Id == id && !r.IsArchived);
            return new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Password = user.Password,
                Email = user.Email,
                IsActive = user.IsActive,
                IsArchived = user.IsArchived,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                UpdatedBy = user.UpdatedBy,
                UpdatedDate = user.UpdatedDate,
                UserName = user.UserName,
                UserRoleMasterId=user.UserRoleMasterId
            };
        }

        public UserViewModel Get(string  memberId)
        {
            var user = RepoUser.Get(r => r.UserName == memberId && !r.IsArchived);
            return new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Password = user.Password,
                Email = user.Email,
                IsActive = user.IsActive,
                IsArchived = user.IsArchived,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                UpdatedBy = user.UpdatedBy,
                UpdatedDate = user.UpdatedDate,
                UserName = user.UserName,
                UserRoleMasterId = user.UserRoleMasterId
            };
        }


        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            var userList = (from user in RepoUser.GetAll()
                            orderby user.FullName
                            select new UserViewModel()
                            {
                                Id = user.Id,
                                FullName = user.FullName,
                                Password = user.Password,
                                Email = user.Email,
                                IsActive = user.IsActive,
                                IsArchived = user.IsArchived,
                                CreatedBy = user.CreatedBy,
                                CreatedDate = user.CreatedDate,
                                UpdatedBy = user.UpdatedBy,
                                UpdatedDate = user.UpdatedDate,
                                UserRoleMasterId = user.UserRoleMasterId,
                                UserName= user.UserName,
                                RoleName=RepoUserRoleMaster.Get(user.UserRoleMasterId).Name,

                            }).ToList();
            return userList;
        }

        public UserViewModel? Login(string userName, string password)
        {
            string passwordHash = Common.HashCode(password); 
            var user = RepoUser.Get(r => r.UserName == userName
                                      && r.Password == passwordHash   
                                      && r.IsActive);

            if (user == null)
            {
                return null;
            }

            return new UserViewModel
            {
                Id = user.Id,
                UserRoleMasterId = user.UserRoleMasterId,
                IsActive = user.IsActive,
                IsArchived = user.IsArchived,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                FullName = user.FullName,
                UserName = user.UserName,
                Email= user.Email
            };
        }


        public IEnumerable<object> GetUserList(int roleId)
        {
            return from r in RepoUser.GetAll(r=> r.UserRoleMasterId== roleId).ToList()
                   select new { Text = r.FullName, Value = r.UserName };
        }
        public IEnumerable<object> GetUserRoleMasterList()
        {
            return from r in RepoUserRoleMaster.GetAll()
                   select new { Text = r.Name, Value = r.Id };
        }

    }
}
