namespace CerberusMultiBranch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchHtml : DbMigration
    {
        public override void Up()
        {
            AddColumn("Config.Branch", "NoteMemberHtml", c => c.String(maxLength: 500));
            AddColumn("Config.Branch", "NoteLocalHtml", c => c.String(maxLength: 500));
            AddColumn("Config.Branch", "LogoPath", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("Config.Branch", "LogoPath");
            DropColumn("Config.Branch", "NoteLocalHtml");
            DropColumn("Config.Branch", "NoteMemberHtml");
        }
    }
}
