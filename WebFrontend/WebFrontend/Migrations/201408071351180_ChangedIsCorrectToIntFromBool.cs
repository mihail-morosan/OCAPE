namespace WebFrontend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedIsCorrectToIntFromBool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OCAPESubmissionReportRuns", "IsCorrect", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OCAPESubmissionReportRuns", "IsCorrect", c => c.Boolean(nullable: false));
        }
    }
}
