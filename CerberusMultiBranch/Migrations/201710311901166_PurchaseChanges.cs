namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("Operative.Transaction", "IDX_Inventoried");
            DropColumn("Operative.Transaction", "Inventoried");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Transaction", "Inventoried", c => c.Boolean());
            CreateIndex("Operative.Transaction", "Inventoried", name: "IDX_Inventoried");
        }
    }
}
