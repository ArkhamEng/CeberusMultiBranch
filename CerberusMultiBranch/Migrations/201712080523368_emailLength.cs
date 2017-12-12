namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("Catalog.Provider", "IDX_Email");
            AlterColumn("Catalog.Employee", "Email", c => c.String(maxLength: 100));
            AlterColumn("Catalog.Client", "Email", c => c.String(maxLength: 100));
            AlterColumn("Catalog.Provider", "WebSite", c => c.String(maxLength: 100));
            AlterColumn("Catalog.Provider", "Email", c => c.String(maxLength: 100));
            CreateIndex("Catalog.Provider", "Email", name: "IDX_Email");
        }
        
        public override void Down()
        {
            DropIndex("Catalog.Provider", "IDX_Email");
            AlterColumn("Catalog.Provider", "Email", c => c.String(maxLength: 30));
            AlterColumn("Catalog.Provider", "WebSite", c => c.String(maxLength: 30));
            AlterColumn("Catalog.Client", "Email", c => c.String(maxLength: 30));
            AlterColumn("Catalog.Employee", "Email", c => c.String(maxLength: 30));
            CreateIndex("Catalog.Provider", "Email", name: "IDX_Email");
        }
    }
}
