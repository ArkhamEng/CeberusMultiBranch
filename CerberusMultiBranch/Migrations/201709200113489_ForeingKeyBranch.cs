namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeingKeyBranch : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.Transaction", "OriginBranch_BranchId", "Config.Branch");
            DropColumn("Operative.Transaction", "OriginBranchId");
            RenameColumn(table: "Operative.Transaction", name: "OriginBranch_BranchId", newName: "OriginBranchId");
            RenameIndex(table: "Operative.Transaction", name: "IX_OriginBranch_BranchId", newName: "IX_OriginBranchId");
            AddForeignKey("Operative.Transaction", "OriginBranchId", "Config.Branch", "BranchId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.Transaction", "OriginBranchId", "Config.Branch");
            RenameIndex(table: "Operative.Transaction", name: "IX_OriginBranchId", newName: "IX_OriginBranch_BranchId");
            RenameColumn(table: "Operative.Transaction", name: "OriginBranchId", newName: "OriginBranch_BranchId");
            AddColumn("Operative.Transaction", "OriginBranchId", c => c.Int());
            AddForeignKey("Operative.Transaction", "OriginBranch_BranchId", "Config.Branch", "BranchId");
        }
    }
}
