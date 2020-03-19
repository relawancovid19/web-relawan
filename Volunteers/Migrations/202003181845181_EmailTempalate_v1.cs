namespace Volunteers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailTempalate_v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        IdEmailTemplate = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.IdEmailTemplate);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailTemplates");
        }
    }
}
