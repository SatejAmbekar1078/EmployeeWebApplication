using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmpWebApp.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required, StringLength(50)]
        public string DepartmentName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
