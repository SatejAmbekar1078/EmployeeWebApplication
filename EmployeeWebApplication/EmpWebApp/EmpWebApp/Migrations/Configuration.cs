using System.Data.Entity.Migrations;
using EmpWebApp.Models;

namespace EmpWebApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // Seed data if necessary
        }
    }
}
