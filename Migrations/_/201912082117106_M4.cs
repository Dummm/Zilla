namespace Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M4 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Teams", newName: "Projects");
            RenameColumn(table: "dbo.AspNetUsers", name: "Team_TeamId", newName: "Project_TeamId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Team_TeamId", newName: "IX_Project_TeamId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Project_TeamId", newName: "IX_Team_TeamId");
            RenameColumn(table: "dbo.AspNetUsers", name: "Project_TeamId", newName: "Team_TeamId");
            RenameTable(name: "dbo.Projects", newName: "Teams");
        }
    }
}
