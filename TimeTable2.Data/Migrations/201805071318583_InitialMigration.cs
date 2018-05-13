namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Classrooms",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RoomId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Capacity = c.Int(nullable: false),
                        Maintenance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.RoomId });
            
            CreateTable(
                "Courses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WeekDay = c.Int(nullable: false),
                        startBlok = c.Int(nullable: false),
                        EndBlok = c.Int(nullable: false),
                        Docent = c.String(unicode: false),
                        VakCode = c.String(unicode: false),
                        Klas = c.String(unicode: false),
                        Classroom_Id = c.Guid(),
                        Classroom_RoomId = c.String(maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Classrooms", t => new { t.Classroom_Id, t.Classroom_RoomId })
                .Index(t => new { t.Classroom_Id, t.Classroom_RoomId });
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("Courses", new[] { "Classroom_Id", "Classroom_RoomId" }, "Classrooms");
            DropIndex("Courses", new[] { "Classroom_Id", "Classroom_RoomId" });
            DropTable("Users");
            DropTable("Courses");
            DropTable("Classrooms");
        }
    }
}
