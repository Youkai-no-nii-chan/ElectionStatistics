using System.Data.Entity.Migrations;

namespace ElectionInfo.Model.Migrations
{
    public class MigrationsConfiguration : DbMigrationsConfiguration<ModelContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            CommandTimeout = 0;
        }
    }
}