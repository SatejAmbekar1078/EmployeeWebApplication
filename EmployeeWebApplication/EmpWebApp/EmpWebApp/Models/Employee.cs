using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmpWebApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<EmployeeRoleMapping> EmployeeRoleMappings { get; set; } = new List<EmployeeRoleMapping>();
    }
}
