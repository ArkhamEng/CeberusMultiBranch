namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdUserClient : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Client", "UpdUser", c => c.String(maxLength: 100));
            DropColumn("Catalog.Client", "UdpUser");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Client", "UdpUser", c => c.String(maxLength: 100));
            DropColumn("Catalog.Client", "UpdUser");
        }
    }
}
