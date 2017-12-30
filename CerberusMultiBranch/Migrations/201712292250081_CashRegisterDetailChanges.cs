namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CashRegisterDetailChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.CashDetail", "WithdrawalCauseId", "Operative.WithdrawalCause");
            AddColumn("Operative.CashDetail", "DetailType", c => c.Int(nullable: false));
            AlterColumn("Operative.CashDetail", "Type", c => c.Int(nullable: false));
            AlterColumn("Operative.CashDetail", "Comment", c => c.String(nullable: false, maxLength: 100));
            AddForeignKey("Operative.CashDetail", "WithdrawalCauseId", "Operative.WithdrawalCause", "WithdrawalCauseId");
            DropColumn("Operative.CashDetail", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("Operative.CashDetail", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("Operative.CashDetail", "WithdrawalCauseId", "Operative.WithdrawalCause");
            AlterColumn("Operative.CashDetail", "Comment", c => c.String(maxLength: 100));
            AlterColumn("Operative.CashDetail", "Type", c => c.Int());
            DropColumn("Operative.CashDetail", "DetailType");
            AddForeignKey("Operative.CashDetail", "WithdrawalCauseId", "Operative.WithdrawalCause", "WithdrawalCauseId", cascadeDelete: false);
        }
    }
}
