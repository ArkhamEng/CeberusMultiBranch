namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class configVar1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.Variable", "Value", c => c.String(maxLength: 15));
            AlterColumn("Config.Variable", "Name", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("Config.Variable", "Name", c => c.Int(nullable: false));
            DropColumn("Config.Variable", "Value");
        }
    }
}
