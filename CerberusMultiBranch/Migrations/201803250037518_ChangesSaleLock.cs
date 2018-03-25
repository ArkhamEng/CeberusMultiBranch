namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesSaleLock : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.Branch", "Code", c => c.String(maxLength: 4));
            AddColumn("Operative.BranchProduct", "LockDate", c => c.DateTime());
            AddColumn("Operative.BranchProduct", "UserLock", c => c.String(maxLength: 30));
            AddColumn("Catalog.Product", "LockDate", c => c.DateTime());
            AddColumn("Catalog.Product", "UserLock", c => c.String(maxLength: 30));
            AddColumn("Operative.Sale", "Year", c => c.Int(nullable: false));
            AddColumn("Operative.Sale", "Sequential", c => c.Int(nullable: false));
            CreateIndex("Operative.BranchProduct", "LockDate", name: "IDX_LockDate");
            CreateIndex("Operative.BranchProduct", "UserLock", name: "IDX_UserLock");
            CreateIndex("Catalog.Product", "LockDate", name: "IDX_LockDate");
            CreateIndex("Catalog.Product", "UserLock", name: "IDX_UserLock");
            CreateIndex("Operative.Sale", "Year", name: "IDX_Year");
            CreateIndex("Operative.Sale", "Sequential", name: "IDX_Sequential");
        }
        
        public override void Down()
        {
            DropIndex("Operative.Sale", "IDX_Sequential");
            DropIndex("Operative.Sale", "IDX_Year");
            DropIndex("Catalog.Product", "IDX_UserLock");
            DropIndex("Catalog.Product", "IDX_LockDate");
            DropIndex("Operative.BranchProduct", "IDX_UserLock");
            DropIndex("Operative.BranchProduct", "IDX_LockDate");
            DropColumn("Operative.Sale", "Sequential");
            DropColumn("Operative.Sale", "Year");
            DropColumn("Catalog.Product", "UserLock");
            DropColumn("Catalog.Product", "LockDate");
            DropColumn("Operative.BranchProduct", "UserLock");
            DropColumn("Operative.BranchProduct", "LockDate");
            DropColumn("Config.Branch", "Code");
        }
    }
}
