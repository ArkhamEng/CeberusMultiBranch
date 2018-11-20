namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressesForPersons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Catalog.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressType = c.String(maxLength: 20),
                        Entity = c.String(maxLength: 20),
                        CityId = c.Int(nullable: false),
                        Location = c.String(nullable: false, maxLength: 150),
                        Street = c.String(nullable: false, maxLength: 150),
                        ZipCode = c.String(maxLength: 10),
                        Reference = c.String(maxLength: 250),
                        ClientId = c.Int(),
                        EmployeeId = c.Int(),
                        ProviderId = c.Int(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("Config.City", t => t.CityId, cascadeDelete: true)
                .ForeignKey("Catalog.Client", t => t.ClientId)
                .ForeignKey("Catalog.Employee", t => t.EmployeeId)
                .ForeignKey("Catalog.Provider", t => t.ProviderId)
                .Index(t => t.CityId, name: "IDX_CityId")
                .Index(t => t.ClientId)
                .Index(t => t.EmployeeId)
                .Index(t => t.ProviderId);
            
            CreateTable(
                "Config.JobPosition",
                c => new
                    {
                        JobPositionId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        UpdUser = c.String(),
                        UpdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.JobPositionId);
            
            AddColumn("Catalog.Client", "Email2", c => c.String(maxLength: 100));
            AddColumn("Catalog.Client", "Phone2", c => c.String(maxLength: 20));
            AddColumn("Catalog.Client", "PersonType", c => c.String());
            AddColumn("Catalog.Client", "LockEndDate", c => c.DateTime());
            AddColumn("Catalog.Client", "LockUser", c => c.String(maxLength: 100));
            AddColumn("Catalog.Employee", "JobPositionId", c => c.Int());
            AddColumn("Catalog.Employee", "LockEndDate", c => c.DateTime());
            AddColumn("Catalog.Employee", "LockUser", c => c.String(maxLength: 100));
            AddColumn("Catalog.Employee", "ComissionForSale", c => c.Int(nullable: false));
            AddColumn("Catalog.Employee", "Salary", c => c.Double(nullable: false));
            AddColumn("Catalog.Provider", "Email2", c => c.String(maxLength: 100));
            AddColumn("Catalog.Provider", "Phone2", c => c.String(maxLength: 20));
            AddColumn("Catalog.Provider", "Phone3", c => c.String(maxLength: 20));
            AddColumn("Catalog.Provider", "LockEndDate", c => c.DateTime());
            AddColumn("Catalog.Provider", "LockUser", c => c.String(maxLength: 100));
            AlterColumn("Catalog.Client", "Address", c => c.String(maxLength: 150));
            AlterColumn("Catalog.Employee", "Address", c => c.String(maxLength: 100));
            AlterColumn("Catalog.Provider", "Address", c => c.String(maxLength: 150));
            AlterColumn("Catalog.Provider", "Phone", c => c.String(maxLength: 20));
            CreateIndex("Catalog.Employee", "JobPositionId");
            AddForeignKey("Catalog.Employee", "JobPositionId", "Config.JobPosition", "JobPositionId");
        }
        
        public override void Down()
        {
            DropForeignKey("Catalog.Address", "ProviderId", "Catalog.Provider");
            DropForeignKey("Catalog.Address", "EmployeeId", "Catalog.Employee");
            DropForeignKey("Catalog.Employee", "JobPositionId", "Config.JobPosition");
            DropForeignKey("Catalog.Address", "ClientId", "Catalog.Client");
            DropForeignKey("Catalog.Address", "CityId", "Config.City");
            DropIndex("Catalog.Employee", new[] { "JobPositionId" });
            DropIndex("Catalog.Address", new[] { "ProviderId" });
            DropIndex("Catalog.Address", new[] { "EmployeeId" });
            DropIndex("Catalog.Address", new[] { "ClientId" });
            DropIndex("Catalog.Address", "IDX_CityId");
            AlterColumn("Catalog.Provider", "Phone", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("Catalog.Provider", "Address", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("Catalog.Employee", "Address", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("Catalog.Client", "Address", c => c.String(nullable: false, maxLength: 150));
            DropColumn("Catalog.Provider", "LockUser");
            DropColumn("Catalog.Provider", "LockEndDate");
            DropColumn("Catalog.Provider", "Phone3");
            DropColumn("Catalog.Provider", "Phone2");
            DropColumn("Catalog.Provider", "Email2");
            DropColumn("Catalog.Employee", "Salary");
            DropColumn("Catalog.Employee", "ComissionForSale");
            DropColumn("Catalog.Employee", "LockUser");
            DropColumn("Catalog.Employee", "LockEndDate");
            DropColumn("Catalog.Employee", "JobPositionId");
            DropColumn("Catalog.Client", "LockUser");
            DropColumn("Catalog.Client", "LockEndDate");
            DropColumn("Catalog.Client", "PersonType");
            DropColumn("Catalog.Client", "Phone2");
            DropColumn("Catalog.Client", "Email2");
            DropTable("Config.JobPosition");
            DropTable("Catalog.Address");
        }
    }
}
