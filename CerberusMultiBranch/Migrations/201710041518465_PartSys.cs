namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartSys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Config.PartSystem",
                c => new
                    {
                        PartSystemId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.PartSystemId);
            
            AddColumn("Catalog.Product", "PartSystemId", c => c.Int());
            CreateIndex("Catalog.Product", "PartSystemId", name: "IDX_PartSystemId");
            AddForeignKey("Catalog.Product", "PartSystemId", "Config.PartSystem", "PartSystemId");
        }
        
        public override void Down()
        {
            DropForeignKey("Catalog.Product", "PartSystemId", "Config.PartSystem");
            DropIndex("Catalog.Product", "IDX_PartSystemId");
            DropColumn("Catalog.Product", "PartSystemId");
            DropTable("Config.PartSystem");
        }
    }
}
