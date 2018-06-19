namespace TimeTable2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserManagement : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "Role", c => c.Int(nullable: false));
            AddColumn("Users", "RoleString", c => c.String(unicode: false));
            AddColumn("Users", "LastLogin", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("Users", "CreatedAt", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("Users", "EmailVerified");
        }
        
        public override void Down()
        {
            AddColumn("Users", "EmailVerified", c => c.Boolean(nullable: false));
            DropColumn("Users", "CreatedAt");
            DropColumn("Users", "LastLogin");
            DropColumn("Users", "RoleString");
            DropColumn("Users", "Role");
        }
    }
}
