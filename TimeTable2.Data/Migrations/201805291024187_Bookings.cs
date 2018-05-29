namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bookings : DbMigration
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
                        StartBlok = c.Int(nullable: false),
                        EndBlok = c.Int(nullable: false),
                        AantalGasten = c.String(unicode: false),
                        LokaalCode = c.String(unicode: false),
                        Owner = c.String(unicode: false),
                        Lokaal_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("Classrooms", t => t.Lokaal_Id)
                .Index(t => t.Lokaal_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Bookings", "Lokaal_Id", "Classrooms");
            DropIndex("Bookings", new[] { "Lokaal_Id" });
            DropTable("Bookings");
        }
    }
}
