namespace LSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreInUserView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.userviews", "Role", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.userviews", "Role");
        }
    }
}
