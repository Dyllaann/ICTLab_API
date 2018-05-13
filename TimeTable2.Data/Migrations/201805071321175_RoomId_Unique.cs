namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoomId_Unique : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Courses", new[] { "Classroom_Id", "Classroom_RoomId" }, "Classrooms");
            DropIndex("Courses", new[] { "Classroom_Id", "Classroom_RoomId" });
            DropPrimaryKey("Classrooms");
            AlterColumn("Classrooms", "RoomId", c => c.String(maxLength: 25, storeType: "nvarchar"));
            AddPrimaryKey("Classrooms", "Id");
            CreateIndex("Classrooms", "RoomId", unique: true, name: "RoomId_Index");
            CreateIndex("Courses", "Classroom_Id");
            AddForeignKey("Courses", "Classroom_Id", "Classrooms", "Id");
            DropColumn("Courses", "Classroom_RoomId");
        }
        
        public override void Down()
        {
            AddColumn("Courses", "Classroom_RoomId", c => c.String(maxLength: 128, storeType: "nvarchar"));
            DropForeignKey("Courses", "Classroom_Id", "Classrooms");
            DropIndex("Courses", new[] { "Classroom_Id" });
            DropIndex("Classrooms", "RoomId_Index");
            DropPrimaryKey("Classrooms");
            AlterColumn("Classrooms", "RoomId", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AddPrimaryKey("Classrooms", new[] { "Id", "RoomId" });
            CreateIndex("Courses", new[] { "Classroom_Id", "Classroom_RoomId" });
            AddForeignKey("Courses", new[] { "Classroom_Id", "Classroom_RoomId" }, "Classrooms", new[] { "Id", "RoomId" });
        }
    }
}
