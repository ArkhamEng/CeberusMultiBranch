namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.Category", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("Config.PartSystem", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("Config.CarModel", "UpdUser", c => c.String(maxLength: 100));
            AddColumn("Config.CarModel", "UpdDate", c => c.DateTime(nullable: false));
            AddColumn("Config.CarModel", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("Config.CarMake", "UpdUser", c => c.String(maxLength: 100));
            AddColumn("Config.CarMake", "UpdDate", c => c.DateTime(nullable: false));
            AddColumn("Config.CarMake", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("Operative.WithdrawalCause", "UpdDate", c => c.DateTime(nullable: false));
            AddColumn("Operative.WithdrawalCause", "UpdUser", c => c.String(maxLength: 100));
            AddColumn("Operative.WithdrawalCause", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("Operative.WithdrawalCause", "Name", c => c.String(maxLength: 50));
            DropColumn("Operative.WithdrawalCause", "UserAdd");
            DropColumn("Operative.WithdrawalCause", "InsDate");
        }
        
        public override void Down()
        {
            AddColumn("Operative.WithdrawalCause", "InsDate", c => c.DateTime(nullable: false));
            AddColumn("Operative.WithdrawalCause", "UserAdd", c => c.String());
            AlterColumn("Operative.WithdrawalCause", "Name", c => c.String());
            DropColumn("Operative.WithdrawalCause", "IsActive");
            DropColumn("Operative.WithdrawalCause", "UpdUser");
            DropColumn("Operative.WithdrawalCause", "UpdDate");
            DropColumn("Config.CarMake", "IsActive");
            DropColumn("Config.CarMake", "UpdDate");
            DropColumn("Config.CarMake", "UpdUser");
            DropColumn("Config.CarModel", "IsActive");
            DropColumn("Config.CarModel", "UpdDate");
            DropColumn("Config.CarModel", "UpdUser");
            DropColumn("Config.PartSystem", "IsActive");
            DropColumn("Config.Category", "IsActive");
        }
    }
}
