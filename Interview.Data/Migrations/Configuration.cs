namespace Interview.Data.Migrations
{

    using System.Data.Entity.Migrations;


    internal sealed class Configuration : DbMigrationsConfiguration<Interview.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //TODO : seed some elements
        }
    }
}
