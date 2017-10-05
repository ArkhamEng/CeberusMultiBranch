namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodeL : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Product", "IDX_Code");
            AlterColumn("Catalog.Product", "Code", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("Catalog.Product", "Code", unique: true, name: "IDX_Code");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Product", "IDX_Code");
            AlterColumn("Catalog.Product", "Code", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("Catalog.Product", "Code", unique: true, name: "IDX_Code");
        }
    }
}
