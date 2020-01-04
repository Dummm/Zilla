namespace Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Project_ProjectId", "dbo.Projects");
            DropIndex("dbo.AspNetUsers", new[] { "Project_ProjectId" });
            CreateTable(
                "dbo.ProjectsOrganizers",
                c => new
                    {
                        ProjectsId = c.Int(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectsId, t.ApplicationUserId })
                .ForeignKey("dbo.Projects", t => t.ProjectsId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ProjectsId)
                .Index(t => t.ApplicationUserId);
            
            DropColumn("dbo.AspNetUsers", "Project_ProjectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Project_ProjectId", c => c.Int());
            DropForeignKey("dbo.ProjectsOrganizers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectsOrganizers", "ProjectsId", "dbo.Projects");
            DropIndex("dbo.ProjectsOrganizers", new[] { "ApplicationUserId" });
            DropIndex("dbo.ProjectsOrganizers", new[] { "ProjectsId" });
            DropTable("dbo.ProjectsOrganizers");
            CreateIndex("dbo.AspNetUsers", "Project_ProjectId");
            AddForeignKey("dbo.AspNetUsers", "Project_ProjectId", "dbo.Projects", "ProjectId");
        }
    }
}
