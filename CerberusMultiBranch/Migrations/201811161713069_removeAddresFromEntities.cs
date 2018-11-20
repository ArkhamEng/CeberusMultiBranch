namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeAddresFromEntities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Catalog.Employee", "CityId", "Config.City");
            DropForeignKey("Catalog.Provider", "CityId", "Config.City");
            DropForeignKey("Catalog.Client", "CityId", "Config.City");
            DropIndex("Catalog.Client", "IDX_CityId");
            DropIndex("Catalog.Employee", "IDX_CityId");
            DropIndex("Catalog.Provider", "IDX_CityId");
            AddColumn("Catalog.Provider", "UpdUser", c => c.String(maxLength: 100));
            AlterColumn("Catalog.Client", "CityId", c => c.Int());
            AlterColumn("Catalog.Employee", "CityId", c => c.Int());
            AlterColumn("Catalog.Provider", "CityId", c => c.Int());
            DropColumn("Catalog.Provider", "UdpUser");
        }
        
        public override void Down()
        {
            AddColumn("Catalog.Provider", "UdpUser", c => c.String(maxLength: 100));
            AlterColumn("Catalog.Provider", "CityId", c => c.Int(nullable: false));
            AlterColumn("Catalog.Employee", "CityId", c => c.Int(nullable: false));
            AlterColumn("Catalog.Client", "CityId", c => c.Int(nullable: false));
            DropColumn("Catalog.Provider", "UpdUser");
            CreateIndex("Catalog.Provider", "CityId", name: "IDX_CityId");
            CreateIndex("Catalog.Employee", "CityId", name: "IDX_CityId");
            CreateIndex("Catalog.Client", "CityId", name: "IDX_CityId");
            AddForeignKey("Catalog.Client", "CityId", "Config.City", "CityId", cascadeDelete: true);
            AddForeignKey("Catalog.Provider", "CityId", "Config.City", "CityId", cascadeDelete: true);
            AddForeignKey("Catalog.Employee", "CityId", "Config.City", "CityId", cascadeDelete: true);
        }
    }
}
