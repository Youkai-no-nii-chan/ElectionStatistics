using System;
using System.Data.Entity.Migrations;

namespace ElectionInfo.Model.Migrations
{
    public partial class ChangeHierarchyPath : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ElectoralDistricts", "IX_HierarchyPath");
            AlterColumn("dbo.ElectoralDistricts", "HierarchyPath", c => c.String(maxLength: 100, unicode: false, nullable: true));

            Sql(@"
update ElectoralDistricts
set HierarchyPath = null
where HierarchyPath = '1'

update ElectoralDistricts
set HierarchyPath = replace(HierarchyPath , '\' + convert(varchar(20), Id), '') 
where HierarchyPath is not null");

            CreateIndex("dbo.ElectoralDistricts", "HierarchyPath", unique: false);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ElectoralDistricts", "IX_HierarchyPath");
            AlterColumn("dbo.ElectoralDistricts", "HierarchyPath", c => c.String(maxLength: 100, unicode: false, nullable: false));
            CreateIndex("dbo.ElectoralDistricts", "HierarchyPath", unique: true);
        }
    }
}
