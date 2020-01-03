namespace Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M5 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TeamMembers", newName: "ProjectsMembers");
            DropForeignKey("dbo.TeamMembers", "TeamId", "dbo.Projects");
            DropForeignKey("dbo.AspNetUsers", "Project_TeamId", "dbo.Projects");
            DropForeignKey("dbo.Assignments", "Team_TeamId", "dbo.Projects");
            RenameColumn(table: "dbo.Assignments", name: "Team_TeamId", newName: "Team_ProjectId");
            RenameColumn(table: "dbo.ProjectsMembers", name: "TeamId", newName: "ProjectsId");
            RenameColumn(table: "dbo.AspNetUsers", name: "Project_TeamId", newName: "Project_ProjectId");
            RenameIndex(table: "dbo.Assignments", name: "IX_Team_TeamId", newName: "IX_Team_ProjectId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Project_TeamId", newName: "IX_Project_ProjectId");
            RenameIndex(table: "dbo.ProjectsMembers", name: "IX_TeamId", newName: "IX_ProjectsId");
            DropPrimaryKey("dbo.Projects");
            AddColumn("dbo.Projects", "ProjectId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Projects", "ProjectId");
            AddForeignKey("dbo.ProjectsMembers", "ProjectsId", "dbo.Projects", "ProjectId", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "Project_ProjectId", "dbo.Projects", "ProjectId");
            AddForeignKey("dbo.Assignments", "Team_ProjectId", "dbo.Projects", "ProjectId");
            DropColumn("dbo.Projects", "TeamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "TeamId", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Assignments", "Team_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.AspNetUsers", "Project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectsMembers", "ProjectsId", "dbo.Projects");
            DropPrimaryKey("dbo.Projects");
            DropColumn("dbo.Projects", "ProjectId");
            AddPrimaryKey("dbo.Projects", "TeamId");
            RenameIndex(table: "dbo.ProjectsMembers", name: "IX_ProjectsId", newName: "IX_TeamId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Project_ProjectId", newName: "IX_Project_TeamId");
            RenameIndex(table: "dbo.Assignments", name: "IX_Team_ProjectId", newName: "IX_Team_TeamId");
            RenameColumn(table: "dbo.AspNetUsers", name: "Project_ProjectId", newName: "Project_TeamId");
            RenameColumn(table: "dbo.ProjectsMembers", name: "ProjectsId", newName: "TeamId");
            RenameColumn(table: "dbo.Assignments", name: "Team_ProjectId", newName: "Team_TeamId");
            AddForeignKey("dbo.Assignments", "Team_TeamId", "dbo.Projects", "TeamId");
            AddForeignKey("dbo.AspNetUsers", "Project_TeamId", "dbo.Projects", "TeamId");
            AddForeignKey("dbo.TeamMembers", "TeamId", "dbo.Projects", "TeamId", cascadeDelete: true);
            RenameTable(name: "dbo.ProjectsMembers", newName: "TeamMembers");
        }
    }
}
