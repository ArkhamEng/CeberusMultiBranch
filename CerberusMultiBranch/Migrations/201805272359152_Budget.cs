namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Budget : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.BudgetDetail",
                c => new
                    {
                        BudgetId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        TaxPercentage = c.Double(nullable: false),
                        TaxAmount = c.Double(nullable: false),
                        TaxedPrice = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Amount = c.Double(nullable: false),
                        TaxedAmount = c.Double(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.BudgetId, t.ProductId })
                .ForeignKey("Operative.Budget", t => t.BudgetId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BudgetId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Operative.Budget",
                c => new
                    {
                        BudgetId = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        BudgetDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        UserName = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.BudgetId)
                .ForeignKey("Config.Branch", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("Catalog.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.ClientId);
            
            AddColumn("Operative.ShoppingCart", "BudgetId", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.BudgetDetail", "ProductId", "Catalog.Product");
            DropForeignKey("Operative.BudgetDetail", "BudgetId", "Operative.Budget");
            DropForeignKey("Operative.Budget", "ClientId", "Catalog.Client");
            DropForeignKey("Operative.Budget", "BranchId", "Config.Branch");
            DropIndex("Operative.Budget", new[] { "ClientId" });
            DropIndex("Operative.Budget", new[] { "BranchId" });
            DropIndex("Operative.BudgetDetail", new[] { "ProductId" });
            DropIndex("Operative.BudgetDetail", new[] { "BudgetId" });
            DropColumn("Operative.ShoppingCart", "BudgetId");
            DropTable("Operative.Budget");
            DropTable("Operative.BudgetDetail");
        }
    }
}
