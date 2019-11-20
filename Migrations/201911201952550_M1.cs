namespace Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Task_Id = c.Int(),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectTasks", t => t.Task_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Task_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DisplayName = c.String(nullable: false),
                        Description = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
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
                        Project_Id = c.Int(),
                        Project_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id1)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Project_Id)
                .Index(t => t.Project_Id1);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Assignee_Id = c.String(nullable: false, maxLength: 128),
                        Assigner_Id = c.String(nullable: false, maxLength: 128),
                        Project_Id = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Assignee_Id, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.Assigner_Id, cascadeDelete: false)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Assignee_Id)
                .Index(t => t.Assigner_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.TeamApplicationUsers",
                c => new
                    {
                        Team_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Team_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Team_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectTasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.TeamApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamApplicationUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.ProjectTasks", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.AspNetUsers", "Project_Id1", "dbo.Projects");
            DropForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Comments", "Task_Id", "dbo.ProjectTasks");
            DropForeignKey("dbo.ProjectTasks", "Assigner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectTasks", "Assignee_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TeamApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.TeamApplicationUsers", new[] { "Team_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Projects", new[] { "Team_Id" });
            DropIndex("dbo.ProjectTasks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProjectTasks", new[] { "Project_Id" });
            DropIndex("dbo.ProjectTasks", new[] { "Assigner_Id" });
            DropIndex("dbo.ProjectTasks", new[] { "Assignee_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id1" });
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "Author_Id" });
            DropIndex("dbo.Comments", new[] { "Task_Id" });
            DropTable("dbo.TeamApplicationUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Teams");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
        }
    }
}
