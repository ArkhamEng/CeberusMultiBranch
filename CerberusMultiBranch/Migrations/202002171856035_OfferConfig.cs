namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Operative.Offer",
                c => new
                    {
                        OfferId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        ImagePath = c.String(),
                        TextColor = c.String(),
                        ShadowColor = c.String(),
                        Discount = c.Int(nullable: false),
                        MinQty = c.Int(nullable: false),
                        MaxQty = c.Int(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Year = c.Int(nullable: false),
                        Consecutive = c.Int(nullable: false),
                        InsDate = c.DateTime(nullable: false),
                        InsUser = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.OfferId)
                .Index(t => t.BeginDate, name: "IDX_BeginDate")
                .Index(t => t.EndDate, name: "IDX_EndDate");
            
        }
        
        public override void Down()
        {
            DropIndex("Operative.Offer", "IDX_EndDate");
            DropIndex("Operative.Offer", "IDX_BeginDate");
            DropTable("Operative.Offer");
        }
    }
}
