namespace Volunteers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTransactionV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Descriptions = c.String(),
                        Banner = c.String(),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        Start = c.DateTimeOffset(nullable: false, precision: 7),
                        Finish = c.DateTimeOffset(nullable: false, precision: 7),
                        Amount = c.Int(nullable: false),
                        Organization_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.JobTransactions",
                c => new
                    {
                        IdTransaction = c.String(nullable: false, maxLength: 128),
                        Registered = c.DateTimeOffset(nullable: false, precision: 7),
                        Status = c.Int(nullable: false),
                        Job_Id = c.String(maxLength: 128),
                        Volunteer_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IdTransaction)
                .ForeignKey("dbo.Jobs", t => t.Job_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Volunteer_Id)
                .Index(t => t.Job_Id)
                .Index(t => t.Volunteer_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .Index(t => t.Id)
                .Index(t => t.CreatedBy_Id);
            
            DropColumn("dbo.AspNetUsers", "SubscribeNewsletter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "SubscribeNewsletter", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Organizations", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Organizations", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobTransactions", "Volunteer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobTransactions", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.Organizations", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Organizations", new[] { "Id" });
            DropIndex("dbo.JobTransactions", new[] { "Volunteer_Id" });
            DropIndex("dbo.JobTransactions", new[] { "Job_Id" });
            DropIndex("dbo.Jobs", new[] { "Organization_Id" });
            DropTable("dbo.Organizations");
            DropTable("dbo.JobTransactions");
            DropTable("dbo.Jobs");
        }
    }
}
