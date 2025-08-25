using DemoTask.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTask.Domain.DomainObject
{
    public class MemberTask : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string MemberId { get; set; }
        public string Status { get; set; }
        public DateTime AssignDate { get; set; }
        public Nullable<DateTime> WorkStartDate { get; set; }
        public Nullable<DateTime> WorkEndDate { get; set; }
        public string? Duration { get; set; }
        public string? Comments { get; set; }
        public string? Remarks { get; set; }
    }
}
