namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodeRef : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Product", "Reference", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Product", "Reference");
        }
    }
}
