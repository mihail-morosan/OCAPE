namespace WebFrontend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.OCAPESubmissionReportRuns",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Test = c.Int(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                        MemoryUsage = c.Int(nullable: false),
                        TimePassed = c.Int(nullable: false),
                        Report_SubmissionID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OCAPESubmissionReports", t => t.Report_SubmissionID)
                .Index(t => t.ID)
                .Index(t => t.Report_SubmissionID);
            
            CreateTable(
                "dbo.OCAPESubmissionReports",
                c => new
                    {
                        SubmissionID = c.Int(nullable: false),
                        Result = c.String(),
                        Score = c.Double(nullable: false),
                        CompiledSuccessfully = c.String(),
                    })
                .PrimaryKey(t => t.SubmissionID)
                .ForeignKey("dbo.OCAPESubmissions", t => t.SubmissionID)
                .Index(t => t.SubmissionID);
            
            CreateTable(
                "dbo.OCAPESubmissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReportID = c.Int(),
                        Filename = c.String(),
                        Extension = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        OwnerNote = c.String(),
                        Owner_Id = c.String(maxLength: 128),
                        Task_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .ForeignKey("dbo.OCAPETasks", t => t.Task_ID)
                .Index(t => t.Owner_Id)
                .Index(t => t.Task_ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OCAPETasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        IsPublic = c.Boolean(nullable: false),
                        IsLeaderboardPublic = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        DeadlineDate = c.DateTime(nullable: false),
                        CompilerToUse = c.String(),
                        IsMaxBoundaryPublic = c.Boolean(nullable: false),
                        MaxMemoryUsage = c.Long(nullable: false),
                        MaxTimeToRun = c.Long(nullable: false),
                        IsWeightingPublic = c.Boolean(nullable: false),
                        MemoryUsageWeight = c.Double(nullable: false),
                        TimeToRunWeight = c.Double(nullable: false),
                        EvaluationsPerTest = c.Double(nullable: false),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OCAPESubmissionReports", "SubmissionID", "dbo.OCAPESubmissions");
            DropForeignKey("dbo.OCAPESubmissions", "Task_ID", "dbo.OCAPETasks");
            DropForeignKey("dbo.OCAPETasks", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OCAPESubmissions", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OCAPESubmissionReportRuns", "Report_SubmissionID", "dbo.OCAPESubmissionReports");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.OCAPETasks", new[] { "Owner_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.OCAPESubmissions", new[] { "Task_ID" });
            DropIndex("dbo.OCAPESubmissions", new[] { "Owner_Id" });
            DropIndex("dbo.OCAPESubmissionReports", new[] { "SubmissionID" });
            DropIndex("dbo.OCAPESubmissionReportRuns", new[] { "Report_SubmissionID" });
            DropIndex("dbo.OCAPESubmissionReportRuns", new[] { "ID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.OCAPETasks");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.OCAPESubmissions");
            DropTable("dbo.OCAPESubmissionReports");
            DropTable("dbo.OCAPESubmissionReportRuns");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
