namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LengthImage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Catalog.Employee", "PictureType", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("Catalog.Employee", "PictureType", c => c.String(maxLength: 8));
        }
    }
}
