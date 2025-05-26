using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//Classe para ser utilizada nos Graficos
namespace TaskManagerV2.Models
{
    public class QuantidadePomodoro
    {
        
        public int Id { get; set; }
        [Key][Column(Order = 0)]
        public string UserId { get; set; }
        [Key][Column(Order = 1)]
        public DateTime Data { get; set; }
        public int QtdPomodoros { get; set; }
    }

    public class QtdPomodorosContext : DbContext
    {
        public DbSet<QuantidadePomodoro> QtdPom { get; set; }
    }
}