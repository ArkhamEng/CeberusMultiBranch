namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FTRchanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Catalog.Provider", "WebSite", c => c.String(maxLength: 150));
            AlterColumn("Catalog.Provider", "FTR", c => c.String(nullable: false, maxLength: 15));
            CreateIndex("Catalog.Provider", "FTR", unique: true, name: "Unq_FTR");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Provider", "Unq_FTR");
            AlterColumn("Catalog.Provider", "FTR", c => c.String(maxLength: 15));
            AlterColumn("Catalog.Provider", "WebSite", c => c.String(maxLength: 100));
        }
    }
}
