namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfigVariableLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Config.Variable", "Value", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("Config.Variable", "Value", c => c.String(maxLength: 15));
        }
    }
}
