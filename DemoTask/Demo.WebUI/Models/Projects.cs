using System.ComponentModel.DataAnnotations;

namespace Demo.WebUI.Models
{
    public class Projects
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string? Remarks { get; set; }
    }
}
