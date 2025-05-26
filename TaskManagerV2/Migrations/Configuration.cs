namespace TaskManagerV2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TaskManagerV2.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TaskManagerV2.Models.TaskDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TaskManagerV2.Models.TaskDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Tasks.AddOrUpdate(i => i.TituloTarefa,
        new Tarefa
        {
            TituloTarefa = "T1",
            DescricaoTarefa = "dt1",
            DataInicio = DateTime.Parse("2014-1-11"),
            DataFinal = DateTime.Parse("2014-1-12"),
            UserID = "ud1",
            Local = "pocos",
            PomodorosPlanejados = 3,
            PomodorosEfetivos = 0,
            Status = 0
        }
        );
        }
    }
}
