namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdUserEmp : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Employee", "UpdUser", c => c.String(maxLength: 100));
            DropColumn("Catalog.Employee", "UdpUser");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Employee", "UdpUser", c => c.String(maxLength: 100));
            DropColumn("Catalog.Employee", "UpdUser");
        }
    }
}
