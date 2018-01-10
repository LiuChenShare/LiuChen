namespace Chenyuan.Data.HR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update20180110p2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreatedById = c.Guid(),
                        JoinDate = c.DateTime(nullable: false),
                        AwayDate = c.DateTime(),
                        LatestJoinDate = c.DateTime(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        EmployeeNo = c.String(),
                        ProbationEndDate = c.DateTime(nullable: false),
                        Salary = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdatedOn = c.DateTime(nullable: false),
                        Timestamp = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmployeeInfo");
        }
    }
}
