namespace LSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Desription", c => c.String());
            DropColumn("dbo.Documents", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "Description", c => c.String());
            DropColumn("dbo.Documents", "Desription");
        }
    }
}
