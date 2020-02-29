namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductIndexes : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Product", "IDX_Ident_TradeMark");
            CreateIndex("Operative.BranchProduct", "Stock", name: "IDX_Stock");
            CreateIndex("Catalog.Product", "Code", name: "IDX_Code");
            CreateIndex("Catalog.Product", "Name", name: "IDX_Name");
            CreateIndex("Catalog.Product", "TradeMark", name: "IDX_TradeMark");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Product", "IDX_TradeMark");
            DropIndex("Catalog.Product", "IDX_Name");
            DropIndex("Catalog.Product", "IDX_Code");
            DropIndex("Operative.BranchProduct", "IDX_Stock");
            CreateIndex("Catalog.Product", new[] { "Code", "Name", "TradeMark" }, name: "IDX_Ident_TradeMark");
        }
    }
}
