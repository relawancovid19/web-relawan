namespace Volunteers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        IdProvince = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Timezone = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdProvince);
            
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Institution", c => c.String());
            AddColumn("dbo.AspNetUsers", "Title", c => c.String());
            AddColumn("dbo.AspNetUsers", "FacebookUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "InstagramUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "TwitterUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "LinkedInUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "YoutubeUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "WebsiteUrl", c => c.String());
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "Registered", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.AspNetUsers", "Updated", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.AspNetUsers", "SubscribeNewsletter", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "IsBanned", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "RegistrationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "InvitationStatus", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Descriptions", c => c.String());
            AddColumn("dbo.AspNetUsers", "InvitedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Province_IdProvince", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "InvitedBy_Id");
            CreateIndex("dbo.AspNetUsers", "Province_IdProvince");
            AddForeignKey("dbo.AspNetUsers", "InvitedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Province_IdProvince", "dbo.Provinces", "IdProvince");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Province_IdProvince", "dbo.Provinces");
            DropForeignKey("dbo.AspNetUsers", "InvitedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Province_IdProvince" });
            DropIndex("dbo.AspNetUsers", new[] { "InvitedBy_Id" });
            DropColumn("dbo.AspNetUsers", "Province_IdProvince");
            DropColumn("dbo.AspNetUsers", "InvitedBy_Id");
            DropColumn("dbo.AspNetUsers", "Descriptions");
            DropColumn("dbo.AspNetUsers", "InvitationStatus");
            DropColumn("dbo.AspNetUsers", "RegistrationStatus");
            DropColumn("dbo.AspNetUsers", "IsBanned");
            DropColumn("dbo.AspNetUsers", "SubscribeNewsletter");
            DropColumn("dbo.AspNetUsers", "Updated");
            DropColumn("dbo.AspNetUsers", "Registered");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "Avatar");
            DropColumn("dbo.AspNetUsers", "WebsiteUrl");
            DropColumn("dbo.AspNetUsers", "YoutubeUrl");
            DropColumn("dbo.AspNetUsers", "LinkedInUrl");
            DropColumn("dbo.AspNetUsers", "TwitterUrl");
            DropColumn("dbo.AspNetUsers", "InstagramUrl");
            DropColumn("dbo.AspNetUsers", "FacebookUrl");
            DropColumn("dbo.AspNetUsers", "Title");
            DropColumn("dbo.AspNetUsers", "Institution");
            DropColumn("dbo.AspNetUsers", "FullName");
            DropTable("dbo.Provinces");
        }
    }
}
