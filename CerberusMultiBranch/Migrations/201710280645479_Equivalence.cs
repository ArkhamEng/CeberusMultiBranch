namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Equivalence : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Catalog.Equivalence", "Code", c => c.String(maxLength: 30));
            CreateIndex("Catalog.Equivalence", "ProviderId", name: "IDX_ProviderId");
            CreateIndex("Catalog.Equivalence", "Code", name: "IDX_Code");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Equivalence", "IDX_Code");
            DropIndex("Catalog.Equivalence", "IDX_ProviderId");
            AlterColumn("Catalog.Equivalence", "Code", c => c.String());
        }
    }
}
