namespace TaskManagerV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tarefas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        TituloTarefa = c.String(),
                        DescricaoTarefa = c.String(),
                        DataInicio = c.DateTime(nullable: false),
                        DataFinal = c.DateTime(nullable: false),
                        Local = c.String(),
                        Pomodoros = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        UserID = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tarefas");
        }
    }
}
