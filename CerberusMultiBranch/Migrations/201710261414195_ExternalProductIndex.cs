namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExternalProductIndex : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "Catalog.ExternalProduct", name: "IX_ProviderId", newName: "IDX_ProviderId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Catalog.ExternalProduct", name: "IDX_ProviderId", newName: "IX_ProviderId");
        }
    }
}
