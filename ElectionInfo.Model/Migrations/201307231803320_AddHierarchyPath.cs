using System;
using System.Data.Entity.Migrations;

namespace ElectionInfo.Model.Migrations
{
    public partial class AddHierarchyPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ElectoralDistricts", "HierarchyPath", c => c.String(maxLength: 100, unicode: false));

            Sql(@"
update ed
set HierarchyPath =
	case when hhhed.HigherDistrictId is null then '' else convert(varchar(20), hhhed.HigherDistrictId) + '\' end + 
	case when hhhed.Id is null then '' else convert(varchar(20), hhhed.Id) + '\' end + 
	case when hhed.Id is null then '' else convert(varchar(20), hhed.Id) + '\' end + 
	case when hed.Id is null then '' else convert(varchar(20), hed.Id) + '\' end + 
	convert(varchar(20), ed.Id)
from ElectoralDistricts ed
left join ElectoralDistricts hed on hed.Id = ed.HigherDistrictId
left join ElectoralDistricts hhed on hhed.Id = hed.HigherDistrictId
left join ElectoralDistricts hhhed on hhhed.Id = hhed.HigherDistrictId");

            AlterColumn("dbo.ElectoralDistricts", "HierarchyPath", c => c.String(maxLength: 100, unicode: false, nullable: false));
            CreateIndex("dbo.ElectoralDistricts", "HierarchyPath", unique: true);
        }
        
        public override void Down()
        {
            DropColumn("dbo.ElectoralDistricts", "HierarchyPath");
        }
    }
}
