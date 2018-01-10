namespace Chenyuan.Data.HR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update20180110p3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeeInfo", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeeInfo", "Name");
        }
    }
}
