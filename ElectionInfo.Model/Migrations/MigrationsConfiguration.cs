using System.Data.Entity.Migrations;

namespace ElectionInfo.Model.Migrations
{
    internal sealed class MigrationsConfiguration : DbMigrationsConfiguration<ModelContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            CommandTimeout = 0;
        }
    }
}