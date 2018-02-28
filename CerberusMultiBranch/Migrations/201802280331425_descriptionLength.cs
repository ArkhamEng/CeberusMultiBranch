namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descriptionLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.ExternalProduct", "IDX_Descripction");
            AlterColumn("Catalog.ExternalProduct", "Description", c => c.String(maxLength: 300));
            CreateIndex("Catalog.ExternalProduct", "Description", name: "IDX_Descripction");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.ExternalProduct", "IDX_Descripction");
            AlterColumn("Catalog.ExternalProduct", "Description", c => c.String(maxLength: 200));
            CreateIndex("Catalog.ExternalProduct", "Description", name: "IDX_Descripction");
        }
    }
}
