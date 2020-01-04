namespace Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Assignment_AssignmentId", "dbo.Assignments");
            DropIndex("dbo.Comments", new[] { "Assignment_AssignmentId" });
            AlterColumn("dbo.Comments", "Assignment_AssignmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "Assignment_AssignmentId");
            AddForeignKey("dbo.Comments", "Assignment_AssignmentId", "dbo.Assignments", "AssignmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Assignment_AssignmentId", "dbo.Assignments");
            DropIndex("dbo.Comments", new[] { "Assignment_AssignmentId" });
            AlterColumn("dbo.Comments", "Assignment_AssignmentId", c => c.Int());
            CreateIndex("dbo.Comments", "Assignment_AssignmentId");
            AddForeignKey("dbo.Comments", "Assignment_AssignmentId", "dbo.Assignments", "AssignmentId");
        }
    }
}
