namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Employee : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Employee", "IDX_BussinessName");
            AddColumn("Catalog.Employee", "Picture", c => c.Binary());
            DropColumn("Catalog.Employee", "BusinessName");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Employee", "BusinessName", c => c.String(maxLength: 50));
            DropColumn("Catalog.Employee", "Picture");
            CreateIndex("Catalog.Employee", "BusinessName", name: "IDX_BussinessName");
        }
    }
}
