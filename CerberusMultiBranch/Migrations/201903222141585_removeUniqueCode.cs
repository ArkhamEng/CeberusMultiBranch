namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUniqueCode : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Product", "UNQ_Code");
        }
        
        public override void Down()
        {
            CreateIndex("Catalog.Product", "Code", unique: true, name: "UNQ_Code");
        }
    }
}
