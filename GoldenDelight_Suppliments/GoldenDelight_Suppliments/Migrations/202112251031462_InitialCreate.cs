namespace GoldenDelight_Suppliments.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        ItemID = c.String(nullable: false, maxLength: 128),
                        CartID = c.String(maxLength: 128),
                        SupID = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ItemID)
                .ForeignKey("dbo.Carts", t => t.CartID)
                .ForeignKey("dbo.Suppliments", t => t.SupID, cascadeDelete: true)
                .Index(t => t.CartID)
                .Index(t => t.SupID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.String(nullable: false, maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CartId);
            
            CreateTable(
                "dbo.Suppliments",
                c => new
                    {
                        SupID = c.Int(nullable: false, identity: true),
                        CategoryID = c.Int(nullable: false),
                        SupName = c.String(),
                        SupDesc = c.String(),
                        SupImage = c.Binary(),
                        Price = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SupID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OItemID = c.Int(nullable: false, identity: true),
                        SupID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OItemID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Suppliments", t => t.SupID, cascadeDelete: true)
                .Index(t => t.SupID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        ProcessStatus = c.String(),
                        PaymentStatus = c.String(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.ClientProfiles", t => t.Email)
                .Index(t => t.Email);
            
            CreateTable(
                "dbo.ClientProfiles",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                        SAID = c.String(),
                        BirthDate = c.String(),
                        Age = c.String(),
                        Gender = c.String(),
                    })
                .PrimaryKey(t => t.Email);
            
            CreateTable(
                "dbo.ClientAddresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        Street = c.String(nullable: false),
                        Suburb = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        Email = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.ClientProfiles", t => t.Email)
                .Index(t => t.Email);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        ShipmentID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        Email = c.String(maxLength: 128),
                        DeliveryDate = c.DateTime(nullable: false),
                        EstimateDuration = c.Int(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ShipmentID)
                .ForeignKey("dbo.ClientProfiles", t => t.Email)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.Email);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        passNumber = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Shipments", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Shipments", "Email", "dbo.ClientProfiles");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CartItems", "SupID", "dbo.Suppliments");
            DropForeignKey("dbo.OrderItems", "SupID", "dbo.Suppliments");
            DropForeignKey("dbo.OrderItems", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Email", "dbo.ClientProfiles");
            DropForeignKey("dbo.ClientAddresses", "Email", "dbo.ClientProfiles");
            DropForeignKey("dbo.Suppliments", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.CartItems", "CartID", "dbo.Carts");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Shipments", new[] { "Email" });
            DropIndex("dbo.Shipments", new[] { "OrderID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ClientAddresses", new[] { "Email" });
            DropIndex("dbo.Orders", new[] { "Email" });
            DropIndex("dbo.OrderItems", new[] { "OrderID" });
            DropIndex("dbo.OrderItems", new[] { "SupID" });
            DropIndex("dbo.Suppliments", new[] { "CategoryID" });
            DropIndex("dbo.CartItems", new[] { "SupID" });
            DropIndex("dbo.CartItems", new[] { "CartID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Shipments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ClientAddresses");
            DropTable("dbo.ClientProfiles");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Categories");
            DropTable("dbo.Suppliments");
            DropTable("dbo.Carts");
            DropTable("dbo.CartItems");
        }
    }
}
