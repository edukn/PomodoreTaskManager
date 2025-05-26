namespace TaskManagerV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tarefas", "PomodorosPlanejados", c => c.Int(nullable: false));
            AddColumn("dbo.Tarefas", "PomodorosEfetivos", c => c.Int(nullable: false));
            DropColumn("dbo.Tarefas", "Pomodoros");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tarefas", "Pomodoros", c => c.Int(nullable: false));
            DropColumn("dbo.Tarefas", "PomodorosEfetivos");
            DropColumn("dbo.Tarefas", "PomodorosPlanejados");
        }
    }
}
