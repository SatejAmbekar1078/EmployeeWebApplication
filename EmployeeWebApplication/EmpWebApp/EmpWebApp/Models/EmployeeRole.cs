using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmpWebApp.Models
{
    public class EmployeeRole
    {
        public int EmployeeRoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public virtual ICollection<EmployeeRoleMapping> EmployeeRoleMappings { get; set; } = new List<EmployeeRoleMapping>();
    }
}
