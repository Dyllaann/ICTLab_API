namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnglishBooking : DbMigration
    {
        public override void Up()
        {
            AddColumn("Bookings", "StartBlock", c => c.Int(nullable: false));
            AddColumn("Bookings", "EndBlock", c => c.Int(nullable: false));
            AddColumn("Bookings", "Guests", c => c.String(unicode: false));
            AddColumn("Bookings", "Classroom", c => c.String(unicode: false));
            DropColumn("Bookings", "StartBlok");
            DropColumn("Bookings", "EndBlok");
            DropColumn("Bookings", "AantalGasten");
            DropColumn("Bookings", "LokaalCode");
        }
        
        public override void Down()
        {
            AddColumn("Bookings", "LokaalCode", c => c.String(unicode: false));
            AddColumn("Bookings", "AantalGasten", c => c.String(unicode: false));
            AddColumn("Bookings", "EndBlok", c => c.Int(nullable: false));
            AddColumn("Bookings", "StartBlok", c => c.Int(nullable: false));
            DropColumn("Bookings", "Classroom");
            DropColumn("Bookings", "Guests");
            DropColumn("Bookings", "EndBlock");
            DropColumn("Bookings", "StartBlock");
        }
    }
}
