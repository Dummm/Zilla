namespace Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectOrganizers", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectOrganizers", "OrganizerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.Assignments", new[] { "Project_Id" });
            DropIndex("dbo.Projects", new[] { "Team_TeamId" });
            DropIndex("dbo.ProjectOrganizers", new[] { "ProjectId" });
            DropIndex("dbo.ProjectOrganizers", new[] { "OrganizerId" });
            AddColumn("dbo.Assignments", "Team_TeamId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Team_TeamId", c => c.Int());
            CreateIndex("dbo.Assignments", "Team_TeamId");
            CreateIndex("dbo.AspNetUsers", "Team_TeamId");
            AddForeignKey("dbo.AspNetUsers", "Team_TeamId", "dbo.Teams", "TeamId");
            AddForeignKey("dbo.Assignments", "Team_TeamId", "dbo.Teams", "TeamId");
            DropColumn("dbo.Assignments", "Project_Id");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectOrganizers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProjectOrganizers",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        OrganizerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.OrganizerId });
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        Team_TeamId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Assignments", "Project_Id", c => c.Int());
            DropForeignKey("dbo.Assignments", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.AspNetUsers", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.AspNetUsers", new[] { "Team_TeamId" });
            DropIndex("dbo.Assignments", new[] { "Team_TeamId" });
            DropColumn("dbo.AspNetUsers", "Team_TeamId");
            DropColumn("dbo.Assignments", "Team_TeamId");
            CreateIndex("dbo.ProjectOrganizers", "OrganizerId");
            CreateIndex("dbo.ProjectOrganizers", "ProjectId");
            CreateIndex("dbo.Projects", "Team_TeamId");
            CreateIndex("dbo.Assignments", "Project_Id");
            AddForeignKey("dbo.Projects", "Team_TeamId", "dbo.Teams", "TeamId");
            AddForeignKey("dbo.ProjectOrganizers", "OrganizerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProjectOrganizers", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Assignments", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
