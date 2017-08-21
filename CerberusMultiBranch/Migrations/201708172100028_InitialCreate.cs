namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inventory.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        MinQuantity = c.Double(nullable: false),
                        BarCode = c.String(),
                        BuyPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "Inventory.ProductInBranch",
                c => new
                    {
                        ProductInBranchId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProductInBranchId);
            
        }
        
        public override void Down()
        {
            DropTable("Inventory.ProductInBranch");
            DropTable("Inventory.Product");
        }
    }
}
