namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class configVar2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Config.Variable", "Name", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("Config.Variable", "Name", c => c.String(maxLength: 15));
        }
    }
}
