namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TempExtProd : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.ExternalProduct", "IDX_Category");
            CreateTable(
                "Catalog.TempExternalProduct",
                c => new
                    {
                        ProviderId = c.Int(nullable: false),
                        Code = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        TradeMark = c.String(),
                        Unit = c.String(),
                    })
                .PrimaryKey(t => new { t.ProviderId, t.Code })
                .Index(t => t.ProviderId, name: "IDX_ProviderId")
                .Index(t => t.Code, name: "IDX_Code");
            
            DropColumn("Catalog.ExternalProduct", "Category");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.ExternalProduct", "Category", c => c.String(nullable: false, maxLength: 60));
            DropIndex("Catalog.TempExternalProduct", "IDX_Code");
            DropIndex("Catalog.TempExternalProduct", "IDX_ProviderId");
            DropTable("Catalog.TempExternalProduct");
            CreateIndex("Catalog.ExternalProduct", "Category", name: "IDX_Category");
        }
    }
}
