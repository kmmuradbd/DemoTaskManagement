using DemoTask.Domain.DomainObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Service.ViewModel
{
    public class ProjectViewModel
    {
        #region Scalar properties
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Start Date is required.")]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "End Date is required.")]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        public string? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string? Remarks { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }

        #endregion

        #region Audit
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        #endregion

        #region Public method
        public Project ToEntity()
        {
            Project project = new Project();

            project.Id = this.Id;
            project.Name = this.Name;
            project.StartDate = this.StartDate;
            project.EndDate = this.EndDate;
            project.ManagerId = this.ManagerId;
            project.Remarks = this.Remarks;
            project.IsActive = this.IsActive;
            project.IsArchived = this.IsArchived;
            project.CreatedBy = this.CreatedBy;
            project.UpdatedBy = this.UpdatedBy;
            project.CreatedDate = this.CreatedDate;
            project.UpdatedDate = this.UpdatedDate;
            return project;
        }
        #endregion
    }
}
