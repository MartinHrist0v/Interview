namespace Interview.Data
{
    using System.Data.Entity;
    using Interview.Data.Models;
    using Migrations;
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext")
        {
            Database.SetInitializer((new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>()));
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<BusinessPartner> BusinessPartners { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }


    }

}