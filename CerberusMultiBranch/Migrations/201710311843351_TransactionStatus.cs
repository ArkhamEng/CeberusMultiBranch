namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionStatus : DbMigration
    {
        public override void Up()
        {
            DropIndex("Operative.Transaction", "IDX_IsPayed");
            AddColumn("Operative.Transaction", "Status", c => c.Int(nullable: false));
            AddColumn("Operative.Transaction", "LastStatus", c => c.Int(nullable: false));
            AddColumn("Operative.Transaction", "Comment", c => c.String(maxLength: 100));
            AddColumn("Operative.Transaction", "UpdUser", c => c.String(nullable: false));
            AlterColumn("Operative.Transaction", "PaymentType", c => c.Int(nullable: false));
            CreateIndex("Operative.Transaction", "Status", name: "IDX_Status");
            DropColumn("Operative.Transaction", "IsPayed");
            DropColumn("Operative.Transaction", "Compleated");
        }
        
        public override void Down()
        {
            AddColumn("Operative.Transaction", "Compleated", c => c.Boolean());
            AddColumn("Operative.Transaction", "IsPayed", c => c.Boolean(nullable: false));
            DropIndex("Operative.Transaction", "IDX_Status");
            AlterColumn("Operative.Transaction", "PaymentType", c => c.Int());
            DropColumn("Operative.Transaction", "UpdUser");
            DropColumn("Operative.Transaction", "Comment");
            DropColumn("Operative.Transaction", "LastStatus");
            DropColumn("Operative.Transaction", "Status");
            CreateIndex("Operative.Transaction", "IsPayed", name: "IDX_IsPayed");
        }
    }
}
