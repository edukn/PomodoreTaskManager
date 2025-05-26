using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskManagerV2.ClassLibrary;
using TaskManagerV2.Models;
using Google.Apis.Services;
using Google.Apis.Calendar.v3.Data;
using System.IO;
using System.Drawing;
using System.Web.Helpers;
//using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;

namespace TaskManagerV2.Controllers
{
    public class TasksController : Controller
    {
        private TaskDbContext db = new TaskDbContext();
        private QtdPomodorosContext db2 = new QtdPomodorosContext();
        // GET: /Tasks/
        public ActionResult Index()
        {
          // string userId = (string)(Session["userId"]);
           string u = User.Identity.Name;
           if (u == "")
               return RedirectToAction("Login", "Account");
           else
           {
               var consult = from Tarefas in db.Tasks
                             select Tarefas;
               consult = consult.Where(s => s.Status.Equals(0));
               consult = consult.Where(x => x.UserID == u);
               return View(consult);
           }
           // return View(consult);
           // return View(db.Tasks.ToList());
        }

        // GET: /Tasks/Details/5
        public ActionResult Details(int? id)
        {
            string u = User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = db.Tasks.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            if (verificaTarefa(id, u) == true) //verificando se a tarefa é do usuario atual
            {
                if (tarefa.Status == 0)
                    ViewBag.Status = "Não concluída";
                else
                    ViewBag.Status = "Concluida";
             return View(tarefa);
            }
            else
                return RedirectToAction("Index");
        }

        // GET: /Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,TituloTarefa,DescricaoTarefa,DataInicio,DataFinal,Local,PomodorosPlanejados")] Tarefa tarefa)
        {
            
            if (ModelState.IsValid)
            {
                tarefa.Status = 0;
                tarefa.UserID = User.Identity.Name;
                db.Tasks.Add(tarefa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tarefa);
        }

        // GET: /Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            string u = User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = db.Tasks.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            if (verificaTarefa(id, u) == true || tarefa.Status == 1) //verificando se a tarefa é do usuario atual
                return View(tarefa);
            else
                return RedirectToAction("Index");
        }

        // POST: /Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,TituloTarefa,DescricaoTarefa,DataInicio,DataFinal,Local,PomodorosPlanejados,Status")] Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                tarefa.UserID = User.Identity.Name;
                db.Entry(tarefa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tarefa);
        }

        // GET: /Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            string u = User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = db.Tasks.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            if (verificaTarefa(id, u) == true) //verificando se a tarefa é do usuario atual
                return View(tarefa);
            else
                return RedirectToAction("Index");
        }

        // POST: /Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tarefa tarefa = db.Tasks.Find(id);
            db.Tasks.Remove(tarefa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Realizar(int? id)
        {
            string u = User.Identity.Name;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = db.Tasks.Find(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            if (tarefa.Status == 1 || verificaTarefa(id, u) == false)
                return RedirectToAction("Index");
            else 
                return View(tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Confirmar Pomodoro")] 
        public ActionResult Realizar(int id)
        {
            Tarefa tarefa = db.Tasks.Find(id);
            tarefa.PomodorosEfetivos = tarefa.PomodorosEfetivos + 1;
            db.Entry(tarefa).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.Message = "Pomodoro Concluído!! Faça uma pausa de 5 minutos!";
            //RegistraPomodoroGraficos(); Beta para registrar a quantidade de pomodoros realizados no dia
            if (tarefa.PomodorosPlanejados <= tarefa.PomodorosEfetivos)
                ViewBag.Message2 = "Você atingiu o número de pomodoros planejados para esta tarefa! |";
            return View(tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(MatchFormKey = "actionn", MatchFormValue = "Terminar Tarefa")]
        public ActionResult Terminar(int id)
        {
            Tarefa tarefa = db.Tasks.Find(id);
            if (tarefa.PomodorosEfetivos != 0)
            {
                tarefa.Status = 1;
                db.Entry(tarefa).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Tarefa Concluída!! |";
                System.Threading.Thread.Sleep(2500);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.M = "Realize pelo menos 1 pomodoro para concluir a tarefa! |";
                return View(tarefa);
            }
        }

        public bool verificaTarefa(int? id, string usr)
        {
            Tarefa tarefa = db.Tasks.Find(id);
            if (tarefa != null)
            {
                if (tarefa.UserID == usr)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public class MultiButtonAttribute : ActionNameSelectorAttribute
        {
            public string MatchFormKey { get; set; }
            public string MatchFormValue { get; set; }

            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                return controllerContext.HttpContext.Request[MatchFormKey] != null &&
                    controllerContext.HttpContext.Request[MatchFormKey] == MatchFormValue;
            }
        }

       
        [Authorize]
        public async Task<ActionResult> ChooseCalendar(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppAuthFlowMetadata()).
                    AuthorizeAsync(cancellationToken);

            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);
            var calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = result.Credential,
                ApplicationName = "Estagio II"
            });
            var calendars = calendarService.CalendarList.List();
            calendars.Fields = "items/id, items/summary, items/location, items/description";
            var list = await calendars.ExecuteAsync();
            var items =
                (from file in list.Items
                 select new CalendarType
                 {
                     Id = file.Id,
                     Title = file.Summary,
                     Location = file.Location,
                     Description = file.Description
                 }).OrderBy(f => f.Title).ToList();
            return View(items);
        }

        [Authorize]
        public async Task<ActionResult> ImportTasks(string t, CancellationToken cancellationToken)
        {
            string id = t;
            var result = await new AuthorizationCodeMvcApp(this, new AppAuthFlowMetadata()).
                    AuthorizeAsync(cancellationToken);

            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);
            var calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = result.Credential,
                ApplicationName = "Estagio II"
            });
            var request = calendarService.Events.List(id);
            var result2 = await request.ExecuteAsync();
            if(result2 != null)
            {
              var items =
                (from file in result2.Items
                 select new CalendarEvent
                 {
                     Id = file.Id,
                     CalendarId = id,
                     Title = file.Summary,
                     Location = file.Location,
                     Description = file.Description,
                     StartDate = DateTime.Parse(file.Start.DateTime.ToString()),
                     EndDate = DateTime.Parse(file.End.DateTime.ToString()),
                    // ColorId = Int32.Parse(file.ColorId)
                     Attendees = file.Attendees != null ? file.Attendees.Select(attende => attende.Email) : null
                 }).OrderBy(f => f.Title).ToList();
              Session["CalendarId"] = t;
              return View(items);
            }
            return View();
        }

        [Authorize]
        public async Task<ActionResult> ImporterView(string t, CancellationToken cancellationToken)
        {
            string tid = t;
            Tarefa tarefa = new Tarefa();
            string cid = Session["CalendarId"].ToString();
            var result = await new AuthorizationCodeMvcApp(this, new AppAuthFlowMetadata()).
                    AuthorizeAsync(cancellationToken);

            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);
            var calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = result.Credential,
                ApplicationName = "Estagio II"
            });
            var calendarEvent = calendarService.Events.Get(cid, tid);
            var result2 = await calendarEvent.ExecuteAsync();
            if(result2 != null)
            {
                tarefa.PomodorosEfetivos = 0;
                tarefa.PomodorosPlanejados = 4;
                tarefa.Status = 0;
                tarefa.UserID = User.Identity.Name;
                tarefa.DataInicio = DateTime.Parse(result2.Start.DateTime.ToString());
                tarefa.TituloTarefa = result2.Summary;
                tarefa.DescricaoTarefa = result2.Description;
                tarefa.DataFinal = DateTime.Parse(result2.End.DateTime.ToString());
                tarefa.Local = result2.Location;
                db.Tasks.Add(tarefa);
                db.SaveChanges();
                ViewBag.Msg = "Tarefa Importada com Sucesso!";
            }
            return View();
        }

        [Authorize]
        public async Task<ActionResult> ExportTasks(int id, CancellationToken cancellationToken)
        {
            Session["tarefaId"] = id;
            var result = await new AuthorizationCodeMvcApp(this, new AppAuthFlowMetadata()).
                    AuthorizeAsync(cancellationToken);

            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);
            var calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = result.Credential,
                ApplicationName = "Estagio II"
            });
            var calendars = calendarService.CalendarList.List();
            calendars.Fields = "items/id, items/summary, items/location, items/description";
            var list = await calendars.ExecuteAsync();
            var items =
                (from file in list.Items
                 select new CalendarType
                 {
                     Id = file.Id,
                     Title = file.Summary,
                     Location = file.Location,
                     Description = file.Description
                 }).OrderBy(f => f.Title).ToList();
            return View(items);
        }

        [Authorize]
        public async Task<ActionResult> ExportView(string t, CancellationToken cancellationToken)
        {
            string calendarId = t;
            int tarefaId = int.Parse(Session["tarefaId"].ToString());
            Tarefa tarefa = db.Tasks.Find(tarefaId);
            Event newEvent = new Event()
            {
                Summary = tarefa.TituloTarefa,
                Location = tarefa.Local,
                Description = tarefa.DescricaoTarefa,
                Start = new EventDateTime() { DateTime = tarefa.DataInicio.ToUniversalTime() },
                End = new EventDateTime() { DateTime = tarefa.DataFinal.ToUniversalTime() }
               // Attendees = null
            };
            var result = await new AuthorizationCodeMvcApp(this, new AppAuthFlowMetadata()).
                    AuthorizeAsync(cancellationToken);

            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);
            var calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = result.Credential,
                ApplicationName = "Estagio II"
            });

            var calendarEvent = calendarService.Events.Insert(newEvent, calendarId);
            var result2 = await calendarEvent.ExecuteAsync();
            return View();
        }

        //[HttpGet]
        //Estava public FileResult Graphics
        public ActionResult Graphics()
        {
          //Chamar função RegistraPomodoroGraficos, corrigir bug que ocorre ao chamar esta função
           //Criar função para pegar dados do Banco e plotar no gráfico abaixo:
           var mychart = new Chart(width: 1361, height: 640, theme: ChartTheme.Blue)
      .AddTitle("GRÁFICO DE PRODUTIVIDADE")
      .AddSeries(
          name: "Pomodoros",
          xValue: new[] { "1/11", "2/11", "3/11", "4/11", "5/11", "6/11", "7/11", "8/11", "9/11", "10/11", "11/11", "12/11", "13/11", "14/11", "15/11", "16/11", "17/11" },
          yValues: new[] { "2", "7", "5", "3", "4", "7", "8", "4", "2", "1", "0", "0", "4", "6", "7", "9", "10" });
           mychart.AddLegend("Legenda");
           
           mychart.Write();

            return View();
        }

        public void RegistraPomodoroGraficos()
        {
            DateTime data = DateTime.Today;
            string u = User.Identity.Name;
            QuantidadePomodoro newQtd = db2.QtdPom.Find(u, data);
            if (newQtd == null)
            {
                QuantidadePomodoro novoQtd = new QuantidadePomodoro();
                novoQtd.Data = DateTime.Today;
                novoQtd.QtdPomodoros = 1;
                novoQtd.UserId = u;
                db2.QtdPom.Add(novoQtd);
                db2.SaveChanges();
            }
            else
            {
                newQtd.QtdPomodoros++;
                db2.Entry(newQtd).State = EntityState.Modified;
                db2.SaveChanges();
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
