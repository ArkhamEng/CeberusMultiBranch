namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Security.AspNetUsers", "Picture", c => c.Binary());
            AddColumn("Security.AspNetUsers", "PictureType", c => c.String());
            DropColumn("Catalog.Employee", "Picture");
            DropColumn("Catalog.Employee", "PictureType");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Employee", "PictureType", c => c.String());
            AddColumn("Catalog.Employee", "Picture", c => c.Binary());
            DropColumn("Security.AspNetUsers", "PictureType");
            DropColumn("Security.AspNetUsers", "Picture");
        }
    }
}
