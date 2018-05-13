namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseWithWeeks : DbMigration
    {
        public override void Up()
        {
            AddColumn("Courses", "Week", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Courses", "Week");
        }
    }
}
