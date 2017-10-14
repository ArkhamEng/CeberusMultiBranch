namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("Operative.Transaction", "IDX_IsCompleated");
            AddColumn("Operative.Transaction", "IsPayed", c => c.Boolean(nullable: false));
            AddColumn("Operative.Transaction", "Expiration", c => c.DateTime());
            AddColumn("Operative.Transaction", "Inventoried", c => c.Boolean());
            AddColumn("Operative.Transaction", "Compleated", c => c.Boolean());
            CreateIndex("Operative.Transaction", "IsPayed", name: "IDX_IsPayed");
            CreateIndex("Operative.Transaction", "Inventoried", name: "IDX_Inventoried");
            DropColumn("Operative.Transaction", "IsCompleated");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Transaction", "IsCompleated", c => c.Boolean(nullable: false));
            DropIndex("Operative.Transaction", "IDX_Inventoried");
            DropIndex("Operative.Transaction", "IDX_IsPayed");
            DropColumn("Operative.Transaction", "Compleated");
            DropColumn("Operative.Transaction", "Inventoried");
            DropColumn("Operative.Transaction", "Expiration");
            DropColumn("Operative.Transaction", "IsPayed");
            CreateIndex("Operative.Transaction", "IsCompleated", name: "IDX_IsCompleated");
        }
    }
}
