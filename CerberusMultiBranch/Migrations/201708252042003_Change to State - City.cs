namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangetoStateCity : DbMigration
    {
        public override void Up()
        {
            AddColumn("Common.City", "Code", c => c.String());
            AddColumn("Common.City", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("Common.State", "Code", c => c.String());
            AddColumn("Common.State", "ShorName", c => c.String());
            AddColumn("Common.State", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Common.State", "IsActive");
            DropColumn("Common.State", "ShorName");
            DropColumn("Common.State", "Code");
            DropColumn("Common.City", "IsActive");
            DropColumn("Common.City", "Code");
        }
    }
}
