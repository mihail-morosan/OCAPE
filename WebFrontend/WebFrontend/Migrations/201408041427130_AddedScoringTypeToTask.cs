namespace WebFrontend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedScoringTypeToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OCAPETasks", "ScoringType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OCAPETasks", "ScoringType");
        }
    }
}
