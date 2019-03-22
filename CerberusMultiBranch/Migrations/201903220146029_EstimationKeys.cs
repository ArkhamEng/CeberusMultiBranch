namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstimationKeys : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Purchasing.PurchaseItem");
            AddPrimaryKey("Purchasing.PurchaseItem", new[] { "UserId", "BranchId", "ProductId", "ProviderId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("Purchasing.PurchaseItem");
            AddPrimaryKey("Purchasing.PurchaseItem", new[] { "UserId", "BranchId", "ProductId" });
        }
    }
}
