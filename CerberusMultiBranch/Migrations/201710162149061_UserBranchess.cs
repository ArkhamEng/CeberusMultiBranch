namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserBranchess : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Config.UserBranch",
                c => new
                    {
                        BranchId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.BranchId, t.UserId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Config.UserBranch", "UserId", "Security.AspNetUsers");
            DropForeignKey("Config.UserBranch", "BranchId", "Config.Branch");
            DropIndex("Config.UserBranch", new[] { "UserId" });
            DropIndex("Config.UserBranch", new[] { "BranchId" });
            DropTable("Config.UserBranch");
        }
    }
}
