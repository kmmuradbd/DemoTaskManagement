using DemoTask.Domain.DomainObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.ViewModel
{
    public class MasterMenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public string URL { get; set; }
        public int SortNo { get; set; }
        public string Icon { get; set; }
        #region Audit
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        #endregion

        public MasterMenu ToEntity()
        {
            MasterMenu masterMenu = new MasterMenu();
            masterMenu.Id = Id;
            masterMenu.Name = Name;
            masterMenu.ParentId = ParentId;
            masterMenu.URL = URL;
            masterMenu.SortNo = SortNo;
            masterMenu.Icon = Icon;
            masterMenu.CreatedDate = CreatedDate;
            masterMenu.UpdatedDate = UpdatedDate;
            masterMenu.CreatedBy = CreatedBy;
            masterMenu.UpdatedBy = UpdatedBy;
            return masterMenu;
        }
    }
}
