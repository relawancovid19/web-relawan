namespace Volunteers.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeployV1 : DbMigration
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
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Descriptions = c.String(),
                        Banner = c.String(),
                        Location = c.String(),
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
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Institution = c.String(),
                        Title = c.String(),
                        FacebookUrl = c.String(),
                        InstagramUrl = c.String(),
                        TwitterUrl = c.String(),
                        LinkedInUrl = c.String(),
                        YoutubeUrl = c.String(),
                        WebsiteUrl = c.String(),
                        Avatar = c.String(),
                        Address = c.String(),
                        Registered = c.DateTimeOffset(nullable: false, precision: 7),
                        Updated = c.DateTimeOffset(nullable: false, precision: 7),
                        IsBanned = c.Boolean(nullable: false),
                        RegistrationStatus = c.Int(nullable: false),
                        InvitationStatus = c.Int(nullable: false),
                        Descriptions = c.String(),
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
                        InvitedBy_Id = c.String(maxLength: 128),
                        Province_IdProvince = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.InvitedBy_Id)
                .ForeignKey("dbo.Provinces", t => t.Province_IdProvince)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.InvitedBy_Id)
                .Index(t => t.Province_IdProvince);
            
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
                "dbo.Provinces",
                c => new
                    {
                        IdProvince = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Timezone = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdProvince);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organizations", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Organizations", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.JobTransactions", "Volunteer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.JobTransactions", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Province_IdProvince", "dbo.Provinces");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "InvitedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Organizations", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Organizations", new[] { "Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.JobTransactions", new[] { "Volunteer_Id" });
            DropIndex("dbo.JobTransactions", new[] { "Job_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Province_IdProvince" });
            DropIndex("dbo.AspNetUsers", new[] { "InvitedBy_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Jobs", new[] { "Organization_Id" });
            DropTable("dbo.Organizations");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.JobTransactions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Provinces");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Jobs");
            DropTable("dbo.EmailTemplates");
        }
    }
}
