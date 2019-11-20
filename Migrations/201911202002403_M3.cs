namespace Zilla.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "RegistrationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "RegistrationDate", c => c.DateTime(nullable: false));
        }
    }
}
