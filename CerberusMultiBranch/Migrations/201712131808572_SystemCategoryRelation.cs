namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemCategoryRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Config.SystemCategory",
                c => new
                    {
                        PartSystemId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        UpdDate = c.DateTime(nullable: false),
                        UpdUser = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => new { t.PartSystemId, t.CategoryId })
                .ForeignKey("Config.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("Config.PartSystem", t => t.PartSystemId, cascadeDelete: true)
                .Index(t => t.PartSystemId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Config.SystemCategory", "PartSystemId", "Config.PartSystem");
            DropForeignKey("Config.SystemCategory", "CategoryId", "Config.Category");
            DropIndex("Config.SystemCategory", new[] { "CategoryId" });
            DropIndex("Config.SystemCategory", new[] { "PartSystemId" });
            DropTable("Config.SystemCategory");
        }
    }
}
