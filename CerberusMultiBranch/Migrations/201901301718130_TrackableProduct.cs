namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackableProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Product", "IsTrackable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Product", "IsTrackable");
        }
    }
}
