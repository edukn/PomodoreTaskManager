using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace TaskManagerV2.Models
{
    public class Tarefa
    {
        
        public string TituloTarefa { get; set; }
        public string DescricaoTarefa { get; set; }

        [Display(Name = "Data de Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataFinal { get; set; }
        public string Local { get; set; }
        public int PomodorosPlanejados { get; set; }
        public int PomodorosEfetivos { get; set; }
        public int Status { get; set; }
        public string UserID { get; set; }
        [Key]
        public int id { get; set; }
    }

    public class TaskDbContext: DbContext
    {
        public DbSet<Tarefa> Tasks { get; set; }
    }
}