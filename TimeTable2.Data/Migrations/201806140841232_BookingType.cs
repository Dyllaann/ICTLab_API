namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookingType : DbMigration
    {
        public override void Up()
        {
            AddColumn("Bookings", "Type", c => c.Int(nullable: false));
            AddColumn("Bookings", "TypeString", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Bookings", "TypeString");
            DropColumn("Bookings", "Type");
        }
    }
}
