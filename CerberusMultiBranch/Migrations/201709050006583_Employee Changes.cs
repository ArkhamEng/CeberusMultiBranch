namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeChanges : DbMigration
    {
        public override void Up()
        {
            DropColumn("Catalog.Employee", "TaxAddress");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Employee", "TaxAddress", c => c.String());
        }
    }
}
