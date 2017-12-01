namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Providerchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("Catalog.Provider", "AgentPhone", c => c.String(maxLength: 20));
            AddColumn("Catalog.Provider", "Agent", c => c.String(maxLength: 100));
            AddColumn("Catalog.Provider", "Line", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("Catalog.Provider", "Line");
            DropColumn("Catalog.Provider", "Agent");
            DropColumn("Catalog.Provider", "AgentPhone");
        }
    }
}
