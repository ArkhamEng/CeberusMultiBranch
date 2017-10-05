namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class configVar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Config.Variable",
                c => new
                    {
                        VariableId = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VariableId);
            
        }
        
        public override void Down()
        {
            DropTable("Config.Variable");
        }
    }
}
