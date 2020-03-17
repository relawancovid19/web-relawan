namespace Volunteers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Job_v1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "Location");
        }
    }
}
