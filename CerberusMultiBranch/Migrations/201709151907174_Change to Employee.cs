namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangetoEmployee : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Employee", "IDX_Phone");
            AddColumn("Catalog.Employee", "Picture", c => c.Binary());
            AddColumn("Catalog.Employee", "PictureType", c => c.String(maxLength: 8));
            AlterColumn("Catalog.Employee", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Catalog.Employee", "Address", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Catalog.Employee", "ZipCode", c => c.String(maxLength: 6));
            AlterColumn("Catalog.Employee", "Phone", c => c.String(nullable: false, maxLength: 12));
            CreateIndex("Catalog.Employee", "Phone", unique: true, name: "IDX_Phone");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Employee", "IDX_Phone");
            AlterColumn("Catalog.Employee", "Phone", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("Catalog.Employee", "ZipCode", c => c.String());
            AlterColumn("Catalog.Employee", "Address", c => c.String(nullable: false));
            AlterColumn("Catalog.Employee", "Name", c => c.String(nullable: false));
            DropColumn("Catalog.Employee", "PictureType");
            DropColumn("Catalog.Employee", "Picture");
            CreateIndex("Catalog.Employee", "Phone", unique: true, name: "IDX_Phone");
        }
    }
}
