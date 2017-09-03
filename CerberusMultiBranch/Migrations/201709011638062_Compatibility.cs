namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Compatibility : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Config.CarYear",
                c => new
                    {
                        CarYearId = c.Int(nullable: false, identity: true),
                        CarModelId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarYearId)
                .ForeignKey("Config.CarModel", t => t.CarModelId, cascadeDelete: true)
                .Index(t => t.CarModelId);
            
            CreateTable(
                "Catalog.Compatibility",
                c => new
                    {
                        CompatibilityId = c.Int(nullable: false, identity: true),
                        CarYearId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompatibilityId)
                .ForeignKey("Config.CarYear", t => t.CarYearId, cascadeDelete: true)
                .ForeignKey("Catalog.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.CarYearId)
                .Index(t => t.ProductId);
            
            AddColumn("Config.CarModel", "CarYearId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("Catalog.Compatibility", "ProductId", "Catalog.Product");
            DropForeignKey("Catalog.Compatibility", "CarYearId", "Config.CarYear");
            DropForeignKey("Config.CarYear", "CarModelId", "Config.CarModel");
            DropIndex("Catalog.Compatibility", new[] { "ProductId" });
            DropIndex("Catalog.Compatibility", new[] { "CarYearId" });
            DropIndex("Config.CarYear", new[] { "CarModelId" });
            DropColumn("Config.CarModel", "CarYearId");
            DropTable("Catalog.Compatibility");
            DropTable("Config.CarYear");
        }
    }
}
