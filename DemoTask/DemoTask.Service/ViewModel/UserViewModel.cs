using DemoTask.Domain.DomainObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public int UserRoleMasterId { get; set; }
        public string UserName { get; set; }
        public User ToEntity()
        {
            User user = new User();
            user.Id = this.Id;
            user.FullName = this.FullName;
            user.UserName = this.UserName;
            user.Password = this.Password;
            user.UserRoleMasterId = this.UserRoleMasterId;
            user.IsActive = this.IsActive;
            user.IsArchived = this.IsArchived;
            user.CreatedBy = this.CreatedBy;
            user.CreatedDate = this.CreatedDate;
            user.UpdatedBy = this.UpdatedBy;
            user.UpdatedDate = this.UpdatedDate;
            return user;
        }
    }
}
