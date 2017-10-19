namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdUserProd : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Product", "UpdUser", c => c.String(maxLength: 100));
            DropColumn("Catalog.Product", "UdpUser");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Product", "UdpUser", c => c.String(maxLength: 100));
            DropColumn("Catalog.Product", "UpdUser");
        }
    }
}
