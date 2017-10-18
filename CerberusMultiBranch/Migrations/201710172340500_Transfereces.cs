namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transfereces : DbMigration
    {
        public override void Up()
        {
            AddColumn("Operative.Transaction", "OriginBranchId", c => c.Int());
            AddColumn("Operative.Transaction", "AuthUser", c => c.String());
            AddColumn("Operative.Transaction", "AuthDate", c => c.DateTime());
            CreateIndex("Operative.Transaction", "OriginBranchId");
            AddForeignKey("Operative.Transaction", "OriginBranchId", "Config.Branch", "BranchId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.Transaction", "OriginBranchId", "Config.Branch");
            DropIndex("Operative.Transaction", new[] { "OriginBranchId" });
            DropColumn("Operative.Transaction", "AuthDate");
            DropColumn("Operative.Transaction", "AuthUser");
            DropColumn("Operative.Transaction", "OriginBranchId");
        }
    }
}
