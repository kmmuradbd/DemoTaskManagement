using DemoTask.Domain.DomainObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.ViewModel
{
    public class MemberTaskViewModel
    {
        #region Scalar properties
        public int Id { get; set; }
        [Required(ErrorMessage = "Project is required")]
        [Display(Name = "Project Name")]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Task Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Member is required")]
        [Display(Name = "Member Name")]
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string Status { get; set; }
        public DateTime AssignDate { get; set; }
        public Nullable<DateTime> WorkStartDate { get; set; }
        public Nullable<DateTime> WorkEndDate { get; set; }
        public String Duration { get; set; }
        public string Comments { get; set; }
        public string Remarks { get; set; }
        public int RoleId { get; set; }
        #endregion

        #region Audit
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        #endregion

        #region Public method
        public MemberTask ToEntity()
        {
            MemberTask memberTask = new MemberTask();

            memberTask.Id = this.Id;
            memberTask.Name = this.Name;
            memberTask.ProjectId = this.ProjectId;
            memberTask.MemberId = this.MemberId;
            memberTask.Status = this.Status;
            memberTask.AssignDate = this.AssignDate;
            memberTask.WorkStartDate = this.WorkStartDate;
            memberTask.WorkEndDate = this.WorkEndDate;
            memberTask.Duration = this.Duration;
            memberTask.Comments = this.Comments;
            memberTask.Remarks = this.Remarks;
            memberTask.IsActive = this.IsActive;
            memberTask.IsArchived = this.IsArchived;
            memberTask.CreatedBy = this.CreatedBy;
            memberTask.UpdatedBy = this.UpdatedBy;
            memberTask.CreatedDate = this.CreatedDate;
            memberTask.UpdatedDate = this.UpdatedDate;
            return memberTask;
        }
        #endregion
    }
}
