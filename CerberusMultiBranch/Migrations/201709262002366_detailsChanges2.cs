namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class detailsChanges2 : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "Operative.TransactionDetail", name: "IDX_TransactionId", newName: "IX_TransactionId");
            RenameIndex(table: "Operative.TransactionDetail", name: "IDX_ProductId", newName: "IX_ProductId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "Operative.TransactionDetail", name: "IX_ProductId", newName: "IDX_ProductId");
            RenameIndex(table: "Operative.TransactionDetail", name: "IX_TransactionId", newName: "IDX_TransactionId");
        }
    }
}
