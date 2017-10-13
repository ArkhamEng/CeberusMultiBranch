namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prov : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Provider", "IDX_Phone");
        }
        
        public override void Down()
        {
            CreateIndex("Catalog.Provider", "Phone", unique: true, name: "IDX_Phone");
        }
    }
}
