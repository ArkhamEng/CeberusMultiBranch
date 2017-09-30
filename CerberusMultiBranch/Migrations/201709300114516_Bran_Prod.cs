namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bran_Prod : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.BranchProduct",
                c => new
                    {
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Stock = c.Double(nullable: false),
                        LastStock = c.Double(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.BranchId, t.ProductId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.BranchProduct", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.BranchProduct", "BranchId", "Config.Branch");
            DropIndex("Operative.BranchProduct", new[] { "ProductId" });
            DropIndex("Operative.BranchProduct", new[] { "BranchId" });
            DropTable("Operative.BranchProduct");
        }
    }
}
