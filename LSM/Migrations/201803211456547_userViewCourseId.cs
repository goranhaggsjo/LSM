namespace LSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userViewCourseId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.userviews", "CourseId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.userviews", "CourseId");
        }
    }
}
