namespace EmpWebApp.Models
{
    public class EmployeeRoleMapping
    {
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        public int EmployeeRoleId { get; set; }
        public virtual EmployeeRole EmployeeRole { get; set; }
    }
}
