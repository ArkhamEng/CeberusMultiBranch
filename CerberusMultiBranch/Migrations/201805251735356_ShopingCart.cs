namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShopingCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.ShoppingCart",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        BranchId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        TaxPercentage = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        TaxedPrice = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        TaxedAmount = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.BranchId, t.ProductId })
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Client", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("Security.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BranchId)
                .Index(t => t.ProductId)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.ShoppingCart", "UserId", "Security.AspNetUsers");
            DropForeignKey("Operative.ShoppingCart", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.ShoppingCart", "ClientId", "Catalog.Client");
            DropForeignKey("Operative.ShoppingCart", "BranchId", "Config.Branch");
            DropIndex("Operative.ShoppingCart", new[] { "ClientId" });
            DropIndex("Operative.ShoppingCart", new[] { "ProductId" });
            DropIndex("Operative.ShoppingCart", new[] { "BranchId" });
            DropIndex("Operative.ShoppingCart", new[] { "UserId" });
            DropTable("Operative.ShoppingCart");
        }
    }
}
