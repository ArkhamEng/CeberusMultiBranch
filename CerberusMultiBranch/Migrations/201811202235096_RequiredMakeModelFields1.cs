namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredMakeModelFields1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Operative.WithdrawalCause", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("Operative.WithdrawalCause", "Name", c => c.String(maxLength: 50));
        }
    }
}
