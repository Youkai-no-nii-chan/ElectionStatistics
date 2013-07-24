namespace ElectionInfo.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500),
                        ShortName = c.String(nullable: false, maxLength: 50),
                        GenitiveName = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Elections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Date = c.DateTime(nullable: false),
                        DataSourceUrl = c.String(nullable: false, maxLength: 1000),
                        ElectoralDistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ElectoralDistricts", t => t.ElectoralDistrictId)
                .Index(t => t.ElectoralDistrictId);
            
            CreateTable(
                "dbo.ElectoralDistricts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 500),
                        HigherDistrictId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ElectoralDistricts", t => t.HigherDistrictId)
                .Index(t => t.HigherDistrictId);
            
            CreateTable(
                "dbo.ElectoralDistrictElections",
                c => new
                    {
                        ElectionId = c.Int(nullable: false),
                        ElectoralDistrictId = c.Int(nullable: false),
                        DataSourceUrl = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => new { t.ElectionId, t.ElectoralDistrictId })
                .ForeignKey("dbo.Elections", t => t.ElectionId)
                .ForeignKey("dbo.ElectoralDistricts", t => t.ElectoralDistrictId)
                .Index(t => t.ElectionId)
                .Index(t => t.ElectoralDistrictId);
            
            CreateTable(
                "dbo.ElectionResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ElectoralDistrictId = c.Int(nullable: false),
                        ElectionId = c.Int(nullable: false),
                        DataSourceUrl = c.String(nullable: false, maxLength: 1000),
                        VotersCount = c.Int(nullable: false),
                        ReceivedBallotsCount = c.Int(nullable: false),
                        EarlyIssuedBallotsCount = c.Int(nullable: false),
                        IssuedInsideBallotsCount = c.Int(nullable: false),
                        IssuedOutsideBallotsCount = c.Int(nullable: false),
                        CanceledBallotsCount = c.Int(nullable: false),
                        OutsideBallotsCount = c.Int(nullable: false),
                        InsideBallotsCount = c.Int(nullable: false),
                        InvalidBallotsCount = c.Int(nullable: false),
                        ValidBallotsCount = c.Int(nullable: false),
                        ReceivedAbsenteeCertificatesCount = c.Int(nullable: false),
                        IssuedAbsenteeCertificatesCount = c.Int(nullable: false),
                        AbsenteeCertificateVotersCount = c.Int(nullable: false),
                        CanceledAbsenteeCertificatesCount = c.Int(nullable: false),
                        IssuedByHigherDistrictAbsenteeCertificatesCount = c.Int(nullable: false),
                        LostAbsenteeCertificatesCount = c.Int(nullable: false),
                        LostBallotsCount = c.Int(nullable: false),
                        UnaccountedBallotsCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ElectoralDistricts", t => t.ElectoralDistrictId)
                .ForeignKey("dbo.Elections", t => t.ElectionId)
                .Index(t => t.ElectoralDistrictId)
                .Index(t => t.ElectionId);
            
            CreateTable(
                "dbo.ElectionCandidateVotes",
                c => new
                    {
                        ElectionResultId = c.Int(nullable: false),
                        CandidateId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ElectionResultId, t.CandidateId })
                .ForeignKey("dbo.ElectionResults", t => t.ElectionResultId)
                .ForeignKey("dbo.Candidates", t => t.CandidateId)
                .Index(t => t.ElectionResultId)
                .Index(t => t.CandidateId);
            
            CreateTable(
                "dbo.ElectionCandidates",
                c => new
                    {
                        Election_Id = c.Int(nullable: false),
                        Candidate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Election_Id, t.Candidate_Id })
                .ForeignKey("dbo.Elections", t => t.Election_Id)
                .ForeignKey("dbo.Candidates", t => t.Candidate_Id)
                .Index(t => t.Election_Id)
                .Index(t => t.Candidate_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ElectionCandidates", new[] { "Candidate_Id" });
            DropIndex("dbo.ElectionCandidates", new[] { "Election_Id" });
            DropIndex("dbo.ElectionCandidateVotes", new[] { "CandidateId" });
            DropIndex("dbo.ElectionCandidateVotes", new[] { "ElectionResultId" });
            DropIndex("dbo.ElectionResults", new[] { "ElectionId" });
            DropIndex("dbo.ElectionResults", new[] { "ElectoralDistrictId" });
            DropIndex("dbo.ElectoralDistrictElections", new[] { "ElectoralDistrictId" });
            DropIndex("dbo.ElectoralDistrictElections", new[] { "ElectionId" });
            DropIndex("dbo.ElectoralDistricts", new[] { "HigherDistrictId" });
            DropIndex("dbo.Elections", new[] { "ElectoralDistrictId" });
            DropForeignKey("dbo.ElectionCandidates", "Candidate_Id", "dbo.Candidates");
            DropForeignKey("dbo.ElectionCandidates", "Election_Id", "dbo.Elections");
            DropForeignKey("dbo.ElectionCandidateVotes", "CandidateId", "dbo.Candidates");
            DropForeignKey("dbo.ElectionCandidateVotes", "ElectionResultId", "dbo.ElectionResults");
            DropForeignKey("dbo.ElectionResults", "ElectionId", "dbo.Elections");
            DropForeignKey("dbo.ElectionResults", "ElectoralDistrictId", "dbo.ElectoralDistricts");
            DropForeignKey("dbo.ElectoralDistrictElections", "ElectoralDistrictId", "dbo.ElectoralDistricts");
            DropForeignKey("dbo.ElectoralDistrictElections", "ElectionId", "dbo.Elections");
            DropForeignKey("dbo.ElectoralDistricts", "HigherDistrictId", "dbo.ElectoralDistricts");
            DropForeignKey("dbo.Elections", "ElectoralDistrictId", "dbo.ElectoralDistricts");
            DropTable("dbo.ElectionCandidates");
            DropTable("dbo.ElectionCandidateVotes");
            DropTable("dbo.ElectionResults");
            DropTable("dbo.ElectoralDistrictElections");
            DropTable("dbo.ElectoralDistricts");
            DropTable("dbo.Elections");
            DropTable("dbo.Candidates");
        }
    }
}
