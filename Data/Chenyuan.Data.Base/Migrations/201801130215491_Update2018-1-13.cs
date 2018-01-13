namespace Chenyuan.Data.Base.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2018113 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Account = c.String(),
                        LastLoginedOn = c.DateTime(),
                        LastLoginIp = c.String(),
                        Email = c.String(),
                        Mobile = c.String(),
                        RegisterCode = c.String(),
                        Password = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdatedOn = c.DateTime(nullable: false),
                        Timestamp = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        SystemId = c.Guid(nullable: false),
                        ParentId = c.Guid(),
                        Deep = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdatedOn = c.DateTime(nullable: false),
                        Timestamp = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdatedOn = c.DateTime(nullable: false),
                        Timestamp = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AccountId = c.Guid(nullable: false),
                        AccountName = c.String(),
                        Name = c.String(),
                        QQ = c.String(),
                        Weixin = c.String(),
                        Weibo = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        LastUpdatedOn = c.DateTime(nullable: false),
                        Timestamp = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserInfo");
            DropTable("dbo.SystemInfo");
            DropTable("dbo.AppInfo");
            DropTable("dbo.AccountInfo");
        }
    }
}
