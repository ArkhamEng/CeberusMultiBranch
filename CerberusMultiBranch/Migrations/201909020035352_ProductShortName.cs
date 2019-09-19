namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductShortName : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Product", "IDX_Ident_TradeMark");
            AddColumn("Catalog.Product", "ShortName", c => c.String(maxLength: 50));
            AddColumn("Catalog.Product", "Comment", c => c.String(maxLength: 300));
            AlterColumn("Catalog.Product", "Name", c => c.String(nullable: false, maxLength: 300));
            CreateIndex("Catalog.Product", new[] { "Code", "Name", "TradeMark" }, name: "IDX_Ident_TradeMark");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Product", "IDX_Ident_TradeMark");
            AlterColumn("Catalog.Product", "Name", c => c.String(nullable: false, maxLength: 200));
            DropColumn("Catalog.Product", "Comment");
            DropColumn("Catalog.Product", "ShortName");
            CreateIndex("Catalog.Product", new[] { "Code", "Name", "TradeMark" }, name: "IDX_Ident_TradeMark");
        }
    }
}
