namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Security.AspNetUsers", "PicturePath", c => c.String());
            AddColumn("Security.AspNetUsers", "ComissionForSale", c => c.Int(nullable: false));
            DropColumn("Security.AspNetUsers", "Picture");
            DropColumn("Security.AspNetUsers", "PictureType");
        }
        
        public override void Down()
        {
            AddColumn("Security.AspNetUsers", "PictureType", c => c.String());
            AddColumn("Security.AspNetUsers", "Picture", c => c.Binary());
            DropColumn("Security.AspNetUsers", "ComissionForSale");
            DropColumn("Security.AspNetUsers", "PicturePath");
        }
    }
}
