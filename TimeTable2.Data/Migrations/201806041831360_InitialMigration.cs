namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Bookings",
                c => new
                    {
                        Key = c.Guid(nullable: false, identity: true),
                        Week = c.Int(nullable: false),
                        WeekDay = c.Int(nullable: false),
                        StartBlock = c.Int(nullable: false),
                        EndBlock = c.Int(nullable: false),
                        Guests = c.String(unicode: false),
                        Classroom = c.String(unicode: false),
                        Owner = c.String(unicode: false),
                        Lokaal_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("Classrooms", t => t.Lokaal_Id)
                .Index(t => t.Lokaal_Id);
            
            CreateTable(
                "Classrooms",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RoomId = c.String(maxLength: 25, storeType: "nvarchar"),
                        Capacity = c.Int(nullable: false),
                        Maintenance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.RoomId, unique: true, name: "RoomId_Index");
            
            CreateTable(
                "Courses",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Week = c.Int(nullable: false),
                        WeekDay = c.Int(nullable: false),
                        StartBlock = c.Int(nullable: false),
                        EndBlock = c.Int(nullable: false),
                        Teacher = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        CourseCode = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Classes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Email = c.String(unicode: false),
                        EmailVerified = c.Boolean(nullable: false),
                        Name = c.String(unicode: false),
                        GivenName = c.String(unicode: false),
                        FamilyName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "ClassCourses",
                c => new
                    {
                        Class_Id = c.Guid(nullable: false),
                        Course_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Class_Id, t.Course_Id })
                .ForeignKey("Classes", t => t.Class_Id, cascadeDelete: true)
                .ForeignKey("Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Class_Id)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "CourseClassrooms",
                c => new
                    {
                        Course_Id = c.Guid(nullable: false),
                        Classroom_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_Id, t.Classroom_Id })
                .ForeignKey("Courses", t => t.Course_Id, cascadeDelete: true)
                .ForeignKey("Classrooms", t => t.Classroom_Id, cascadeDelete: true)
                .Index(t => t.Course_Id)
                .Index(t => t.Classroom_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Bookings", "Lokaal_Id", "Classrooms");
            DropForeignKey("CourseClassrooms", "Classroom_Id", "Classrooms");
            DropForeignKey("CourseClassrooms", "Course_Id", "Courses");
            DropForeignKey("ClassCourses", "Course_Id", "Courses");
            DropForeignKey("ClassCourses", "Class_Id", "Classes");
            DropIndex("CourseClassrooms", new[] { "Classroom_Id" });
            DropIndex("CourseClassrooms", new[] { "Course_Id" });
            DropIndex("ClassCourses", new[] { "Course_Id" });
            DropIndex("ClassCourses", new[] { "Class_Id" });
            DropIndex("Classrooms", "RoomId_Index");
            DropIndex("Bookings", new[] { "Lokaal_Id" });
            DropTable("CourseClassrooms");
            DropTable("ClassCourses");
            DropTable("Users");
            DropTable("Classes");
            DropTable("Courses");
            DropTable("Classrooms");
            DropTable("Bookings");
        }
    }
}
