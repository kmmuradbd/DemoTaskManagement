using Dapper;
using DemoTask.Core;
using DemoTask.Domain.RepositoryContract;
using DemoTask.Service.Interface;
using DemoTask.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.Service
{
    public class MasterMenuService : IMasterMenuService
    {
        protected readonly IMasterMenuRepository RepoMasterMenu;
        protected readonly IDapperService RepoDapper;
        public MasterMenuService(IMasterMenuRepository masterMenu, IDapperService dapper)
        {
            RepoMasterMenu = masterMenu;
            RepoDapper = dapper;
        }
        public void Add(MasterMenuViewModel masterMenu)
        {
            throw new NotImplementedException();
        }
        public void Update(MasterMenuViewModel masterMenu)
        {
            throw new NotImplementedException();
        }

        public MasterMenuViewModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public List<MasterMenuViewModel> GetAll()
        {
            var masterMenus = (from mastermenu in RepoMasterMenu.GetAll(r => !r.IsArchived)
                               select new MasterMenuViewModel()
                               {
                                   Id = mastermenu.Id,
                                   Name = mastermenu.Name,
                                   URL = mastermenu.URL,
                                   ParentId = mastermenu.ParentId,
                                   SortNo = mastermenu.SortNo,
                                   Icon = mastermenu.Icon,
                                   IsActive = mastermenu.IsActive,
                                   CreatedDate = mastermenu.CreatedDate,
                                   CreatedBy = mastermenu.CreatedBy,
                                   UpdatedBy = mastermenu.UpdatedBy,
                                   UpdatedDate = mastermenu.UpdatedDate,
                                   IsArchived = mastermenu.IsArchived
                               }).ToList();
            return masterMenus;
        }
        public List<MasterMenuViewModel> GetAll(string userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userId", userId ?? string.Empty);
                return RepoDapper.GetAll<MasterMenuViewModel>("Sp_GetMenues", parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
