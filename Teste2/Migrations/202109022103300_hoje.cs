namespace Teste2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hoje : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bandas", "MusicoId", c => c.Int());
            AlterColumn("dbo.Bandas", "Dono", c => c.String());
            CreateIndex("dbo.Bandas", "MusicoId");
            AddForeignKey("dbo.Bandas", "MusicoId", "dbo.Musicoes", "MusicoId");
            DropColumn("dbo.Bandas", "NomeDono");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bandas", "NomeDono", c => c.String());
            DropForeignKey("dbo.Bandas", "MusicoId", "dbo.Musicoes");
            DropIndex("dbo.Bandas", new[] { "MusicoId" });
            AlterColumn("dbo.Bandas", "Dono", c => c.Int());
            DropColumn("dbo.Bandas", "MusicoId");
        }
    }
}
