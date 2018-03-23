namespace LSM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentUservDbSet : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.userviews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.userviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        email = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
