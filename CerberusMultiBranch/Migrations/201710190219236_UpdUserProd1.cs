namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdUserProd1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Operative.CashDetail", "Cause_WithdrawalCauseId", "Operative.WithdrawalCause");
            RenameColumn(table: "Operative.CashDetail", name: "Cause_WithdrawalCauseId", newName: "WithdrawalCauseId");
            RenameIndex(table: "Operative.CashDetail", name: "IX_Cause_WithdrawalCauseId", newName: "IX_WithdrawalCauseId");
            AddForeignKey("Operative.CashDetail", "WithdrawalCauseId", "Operative.WithdrawalCause", "WithdrawalCauseId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Operative.CashDetail", "WithdrawalCauseId", "Operative.WithdrawalCause");
            RenameIndex(table: "Operative.CashDetail", name: "IX_WithdrawalCauseId", newName: "IX_Cause_WithdrawalCauseId");
            RenameColumn(table: "Operative.CashDetail", name: "WithdrawalCauseId", newName: "Cause_WithdrawalCauseId");
            AddForeignKey("Operative.CashDetail", "Cause_WithdrawalCauseId", "Operative.WithdrawalCause", "WithdrawalCauseId");
        }
    }
}
