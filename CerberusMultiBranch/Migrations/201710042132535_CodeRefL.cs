namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodeRefL : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Catalog.Product", "Reference", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("Catalog.Product", "Reference", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
