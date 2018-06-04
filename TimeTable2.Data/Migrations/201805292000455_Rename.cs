namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename : DbMigration
    {
        public override void Up()
        {
            AddColumn("Courses", "EndBlock", c => c.Int(nullable: false));
            AddColumn("Courses", "EndBlock", c => c.Int(nullable: false));
            AddColumn("Courses", "Teacher", c => c.String(unicode: false));
            AddColumn("Courses", "CourseCode", c => c.String(unicode: false));
            AddColumn("Courses", "Class", c => c.String(unicode: false));
            AddColumn("Courses", "Lokaal", c => c.String(unicode: false));
            DropColumn("Courses", "startBlok");
            DropColumn("Courses", "EndBlock");
            DropColumn("Courses", "Docent");
            DropColumn("Courses", "VakCode");
            DropColumn("Courses", "Klas");
        }
        
        public override void Down()
        {
            AddColumn("Courses", "Klas", c => c.String(unicode: false));
            AddColumn("Courses", "VakCode", c => c.String(unicode: false));
            AddColumn("Courses", "Docent", c => c.String(unicode: false));
            AddColumn("Courses", "EndBlock", c => c.Int(nullable: false));
            AddColumn("Courses", "startBlok", c => c.Int(nullable: false));
            DropColumn("Courses", "Lokaal");
            DropColumn("Courses", "Class");
            DropColumn("Courses", "CourseCode");
            DropColumn("Courses", "Teacher");
            DropColumn("Courses", "EndBlock");
            DropColumn("Courses", "EndBlock");
        }
    }
}
