using System.Data.Entity;
using EmpWebApp.Models;

namespace EmpWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DataConnection") { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<EmployeeRoleMapping> EmployeeRoleMappings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasRequired(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<EmployeeRoleMapping>()
                .HasKey(e => new { e.EmployeeId, e.EmployeeRoleId });

            modelBuilder.Entity<EmployeeRoleMapping>()
                .HasRequired(erm => erm.Employee)
                .WithMany(e => e.EmployeeRoleMappings)
                .HasForeignKey(erm => erm.EmployeeId);

            modelBuilder.Entity<EmployeeRoleMapping>()
                .HasRequired(erm => erm.EmployeeRole)
                .WithMany(er => er.EmployeeRoleMappings)
                .HasForeignKey(erm => erm.EmployeeRoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
