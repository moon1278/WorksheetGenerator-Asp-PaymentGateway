using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorksheetGenerator.Data;
using WorksheetGenerator.Helper;
using WorksheetGenerator.Models.WorksheetModels;
using WorksheetGenerator.Models.WorksheetModels.TaskModels;
using System.Web;
using WorksheetGenerator.Helper.Task;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Web;
using WorksheetGenerator.Helper.Logging;
using System.Dynamic;
using WorksheetGenerator.Helper.Word;
using WorksheetGenerator.Models.SpecificationModels;
using WorksheetGenerator.Helper.Worksheet;
using WorksheetGenerator.Helper.Specification;
using WorksheetGenerator.Models.WorksheetModels.TaskModels.General;
using Task = WorksheetGenerator.Data.Task;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WorksheetGenerator.Models.Task;
using System.Text;

namespace WorksheetGenerator.Controllers
{
    [Authorize]
    public class WorksheetController : Controller
    {
        //private readonly ILogger<WorksheetController> _logger;
        private readonly LogHelper _logger = new LogHelper();
        private WordHelper _word { get; set; }
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public WorksheetController(ILogger<WorksheetController> logger, RoleManager<IdentityRole> roleManager)
        {
            //_logger = logger;
            _word = new WordHelper();
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                var roles = db.Roles.ToList();
                var role = roles.FirstOrDefault();
            }

            return View();
        }

        public async Task<IActionResult> OverviewAsync()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {

            CreateWorksheetViewModel_General model = new CreateWorksheetViewModel_General();

            model.Classes = GetClassesList();
            model.Subjects = GetSubjectList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateWorksheetViewModel_General model)
        {
            if (ModelState.IsValid)
            {
                model.Classes = GetClassesList();
                model.Subjects = GetSubjectList();

                string _worksheetName = model.WorksheetName;

                string _subject = "Fehler";

                foreach (SelectListItem _sSubject in model.Classes)
                {
                    if (_sSubject.Value.ToString().Equals(model.SelectedSubject.ToString()))
                    {
                        _subject = _sSubject.Value;
                    }
                }

                CreateWorksheetViewModel_Task model_task = new CreateWorksheetViewModel_Task();

                model_task.TaskTypes = GetTaskTypeList((int)model.SelectedSubject);
                model_task.SubTaskTypes = new List<SelectListItem>();

                //Transfer from previous page
                model_task.WorksheetName = _worksheetName;
                model_task.ClassId = (int)model.SelectedClass;

                return PartialView("_Create_Task", model_task);
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Task(string obj)
        {
            JsonTasksOfSubType jsonTasks = JsonConvert.DeserializeObject<JsonTasksOfSubType>(obj);

            UserInput userInput = new UserInput();
            OpenAITaskHelper _taskHelper = new OpenAITaskHelper();

            string _worksheetName = jsonTasks.WorksheetName;
            int _classId = Int32.Parse(jsonTasks.ClassId);

            int _worksheetId = -1;
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Worksheet worksheet = new Worksheet();

                worksheet.Name = _worksheetName;
                worksheet.UserId = currentUserID;
                worksheet.Created = DateTime.UtcNow;
                //"d21f0d6c-e979-4bc5-8625-206cfb6b53da"; //TODO update wenn logged in

                db.Worksheets.Add(worksheet);
                db.SaveChangesAsync();

                System.Threading.Thread.Sleep(2000);

                _worksheetId = worksheet.Id;
            }

            foreach (TaskJSONModel _selectTask in jsonTasks.Tasks)
            {
                //userInput = await _taskHelper.GetAIData_ZeitformAnhandSatz_Async(userInput, _worksheetId, (int)EnumTasks.ZeitformAnhanSatz);

                switch (Int32.Parse(_selectTask.TaskId))
                {
                    case (int)EnumTasks.ZeitformAnhandSatz:
                        userInput = await _taskHelper.GetAIData_ZeitformAnhandSatz_Async(userInput, _worksheetId, (int)EnumTasks.ZeitformAnhandSatz, _selectTask.Specifications);


                        break;
                    case (int)EnumTasks.ZeichnenSilbenBoegen:
                        userInput = await _taskHelper.GetAIData_ZeichnenSilbenBoegen_Async(userInput, _worksheetId, (int)EnumTasks.ZeichnenSilbenBoegen, _selectTask.Specifications);


                        break;

                    case (int)EnumTasks.SatzAnfangAustauschen:
                        userInput = await _taskHelper.GetAIData_SatzAnfangAustauschen_Async(userInput, _worksheetId, (int)EnumTasks.SatzAnfangAustauschen, _selectTask.Specifications);

                        break;

                    case (int)EnumTasks.PassendeSatzanfaengefinden:
                        userInput = await _taskHelper.GetAIData_PassendeSatzanfaengefinden_Async(userInput, _worksheetId, (int)EnumTasks.PassendeSatzanfaengefinden, _selectTask.Specifications);

                        break;
                    case (int)EnumTasks.SatzZeitUmschreiben:
                        userInput = await _taskHelper.GetAIData_SatzZeitUmschreiben_Async(userInput, _worksheetId, (int)EnumTasks.SatzZeitUmschreiben, _selectTask.Specifications);

                        break;
                    case (int)EnumTasks.ArtikelEinsaetzen:
                        userInput = await _taskHelper.GetAIData_ArtikelEinsaetzen_Async(userInput, _worksheetId, (int)EnumTasks.ArtikelEinsaetzen, _selectTask.Specifications);

                        break;
                    case (int)EnumTasks.ArtikelVorNomen:
                        userInput = await _taskHelper.GetAIData_ArtikelVorNomen_Async(userInput, _worksheetId, (int)EnumTasks.ArtikelVorNomen, _selectTask.Specifications);

                        break;
                    case (int)EnumTasks.AddiereZahlen100Bis1000:
                        userInput = await _taskHelper.GetAIData_AddiereZahlen100Bis1000_Async(userInput, _worksheetId, (int)EnumTasks.AddiereZahlen100Bis1000, _selectTask.Specifications);
                        break;
                    case (int)EnumTasks.GegenteileFinde:
                        userInput = await _taskHelper.GetAIData_GegenteileFinden_Async(userInput, _worksheetId, (int)EnumTasks.GegenteileFinde, _selectTask.Specifications);
                        break;
                    default:
                        userInput = await _taskHelper.GetAIData_ZeitformAnhandSatz_Async(userInput, _worksheetId, (int)EnumTasks.ZeitformAnhandSatz, _selectTask.Specifications);
                        break;
                }
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                RlWorksheetClass rlWorksheetClass = new RlWorksheetClass();

                rlWorksheetClass.ClassId = Int32.Parse(jsonTasks.ClassId);
                rlWorksheetClass.WorksheetId = _worksheetId;

                db.RlWorksheetClasses.Add(rlWorksheetClass);
                db.SaveChangesAsync();
                System.Threading.Thread.Sleep(4000);
            }

            CreateWorksheetViewModel_WorksheetDownload returnWorksheet = new CreateWorksheetViewModel_WorksheetDownload();
            returnWorksheet.WorksheetId = _worksheetId;

            if (returnWorksheet.WorksheetId == null || returnWorksheet.WorksheetId == -1)
            {
                return View("Error");
            }

            return PartialView("_Create_Done", returnWorksheet);
        }

        [HttpGet]
        public ActionResult DownloadWorksheet(string obj)
        {
            UserInput_And_Worksheet userInputWorksheet = GetUserInputAndWorksheet(obj);

            TaskCreationWordHelper taskHelper = new TaskCreationWordHelper();

            MemoryStream stream = taskHelper.CreateTask(userInputWorksheet.UserInput, userInputWorksheet.Worksheet);

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", userInputWorksheet.Worksheet.Name + ".docx");
        }

        [HttpGet]
        public ActionResult DownloadSolution(string obj)
        {
            UserInput_And_Worksheet userInputWorksheet = GetUserInputAndWorksheet(obj);

            TaskCreationWordHelper taskHelper = new TaskCreationWordHelper();

            MemoryStream stream = taskHelper.CreateSolution(userInputWorksheet.UserInput, userInputWorksheet.Worksheet);

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Loesung_"+userInputWorksheet.Worksheet.Name + ".docx");
        }

        public ActionResult onTaskTypeSelected(string tasktypeId)
        {
            int _taskTypeId;

            if (!Int32.TryParse(tasktypeId, out _taskTypeId))
            {
                return Json("");
            }

            List<SubTaskType> subTaskTypeReturn = new List<SubTaskType>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<SubTaskType> subTaskTypes = db.SubTaskTypes.Where(x => x.TaskTypeId == _taskTypeId).ToList();
                foreach (SubTaskType subTaskType in subTaskTypes)
                {
                    List<Task> tasks = db.Tasks.Where(x => x.SubTaskTypeId == subTaskType.Id).ToList();
                    bool subTaskTypeHasActiveTasks = false;

                    foreach (Task task in tasks)
                    {
                        if (task.Activated == true)
                        {
                            subTaskTypeReturn.Add(subTaskType);
                            subTaskTypeHasActiveTasks = true;
                            break;
                        }
                    }

                  
                }

                string jsonReturn = JsonConvert.SerializeObject(subTaskTypeReturn);
                return Json(jsonReturn);

            }


        
    }

        private UserInput_And_Worksheet GetUserInputAndWorksheet(string obj)
        {
            int _worksheetId;

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!Int32.TryParse(obj, out _worksheetId))
            {
                return null;
            }

            List<RlWorksheetTask> rlTasks;

            Worksheet worksheet = null;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                rlTasks = db.RlWorksheetTasks.Where(r => r.WorksheetId == _worksheetId).ToList();
                worksheet = db.Worksheets.Where(r => r.Id == _worksheetId).FirstOrDefault();
            }

            //TODO Add Error message "Kein Zugriff auf dieses Arbeitsblatt", und füge Ausnahme hinzu für Admins  
            if (worksheet.UserId != currentUserID && !currentUser.IsInRole("Administrator"))
            {
                return null;
            }

            if (rlTasks.Count == 0)
            {
                return null;
            }


            UserInput userInput = new UserInput();

            foreach (RlWorksheetTask rltask in rlTasks)
            {
                switch (rltask.TaskId)
                {
                    case (int)EnumTasks.ZeitformAnhandSatz:

                        List<ZeitformAnhandSatz> zeitenDesVerbens = JsonConvert.DeserializeObject<List<ZeitformAnhandSatz>>(rltask.Result);

                        ZeitformAnhandSatz_Task zeitenDesVerbens_Task = new ZeitformAnhandSatz_Task();
                        zeitenDesVerbens_Task.ZeitformAnhandSatz_Values = zeitenDesVerbens;

                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(zeitenDesVerbens_Task);

                        break;

                    case (int)EnumTasks.ZeichnenSilbenBoegen:

                        List<ZeichnenSilbenBoegen> zeichnenSilbenBoegens = JsonConvert.DeserializeObject<List<ZeichnenSilbenBoegen>>(rltask.Result);

                        ZeichnenSilbenBoegen_Task zeichnenSilbenBoegen_Task = new ZeichnenSilbenBoegen_Task();
                        zeichnenSilbenBoegen_Task.ZeichnenSilbenBoegen_Values = zeichnenSilbenBoegens;

                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(zeichnenSilbenBoegen_Task);

                        break;

                    case (int)EnumTasks.SatzAnfangAustauschen:

                        SingleStringReturn singleStringReturn = JsonConvert.DeserializeObject<SingleStringReturn>(rltask.Result);

                        SingleStringReturn_Task singleStringReturn_Task = new SingleStringReturn_Task();
                        singleStringReturn_Task.SingleStringReturn_Value = singleStringReturn;

                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(singleStringReturn_Task);

                        break;
                    case (int)EnumTasks.PassendeSatzanfaengefinden:

                        List<SingleStringReturn> listString = JsonConvert.DeserializeObject<List<SingleStringReturn>>(rltask.Result);

                        ListStringReturn_Task listStringReturn_Task = new ListStringReturn_Task();
                        listStringReturn_Task.StringList_Values = listString;

                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(listStringReturn_Task);

                        break;
                    
                    case (int)EnumTasks.SatzZeitUmschreiben:

                        List<SatzZeitUmschreiben> sentences_and_solutions = JsonConvert.DeserializeObject<List<SatzZeitUmschreiben>>(rltask.Result);

                        SatzZeitUmschreiben_Task satzZeitUmschreiben_Task = new SatzZeitUmschreiben_Task();
                        satzZeitUmschreiben_Task.SatzZeitUmschreiben_Values = sentences_and_solutions;

                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(satzZeitUmschreiben_Task);

                        break;
                    case (int)EnumTasks.ArtikelEinsaetzen:

                        SingleStringReturn singleString = JsonConvert.DeserializeObject<SingleStringReturn>(rltask.Result);

                        SingleStringReturn_Task stringReturn_Task_2 = new SingleStringReturn_Task();
                        stringReturn_Task_2.SingleStringReturn_Value = singleString;

                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(stringReturn_Task_2);

                        break;
                    case (int)EnumTasks.ArtikelVorNomen:

                        List<SingleStringReturn> saetze = JsonConvert.DeserializeObject<List<SingleStringReturn>>(rltask.Result);

                        ListStringReturn_Task liststringReturn_Task_2 = new ListStringReturn_Task();
                        liststringReturn_Task_2.StringList_Values = saetze;

                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(liststringReturn_Task_2);

                        break;

                    case (int)EnumTasks.GegenteileFinde:

                        ListArrayReturn gegenteileFindenListe = JsonConvert.DeserializeObject<ListArrayReturn>(rltask.Result);

                        ListArrayReturn_Task gegenteileFinden_Task_2 = new ListArrayReturn_Task();
                        gegenteileFinden_Task_2.ListArray_Values = gegenteileFindenListe;

                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(gegenteileFinden_Task_2);
                        break;
                    case (int)EnumTasks.AddiereZahlen100Bis1000:

                        List<AddiereZahlen100Bis1000> aufgaben = JsonConvert.DeserializeObject<List<AddiereZahlen100Bis1000>>(rltask.Result);

                        AddiereZahlen100Bis1000_Task addiereZahlen100Bis1000_Task = new AddiereZahlen100Bis1000_Task();
                        addiereZahlen100Bis1000_Task.AddiereZahlen100Bis1000_Values = aufgaben;
                        
                        //userInput.ZeitenDesVerbensValues.Add(zeitenDesVerbens);
                        userInput.Tasks.Add(addiereZahlen100Bis1000_Task);


                        break;

                    default:
                        break;
                }
            }

            UserInput_And_Worksheet userInput_And_Worksheet = new UserInput_And_Worksheet();
            userInput_And_Worksheet.UserInput = userInput;
            userInput_And_Worksheet.Worksheet = worksheet;

            return userInput_And_Worksheet;
        }
        
        [HttpPost]
        public ActionResult UpdateSubTaskType(string subTaskTypeId)
        {
            int _subTaskTypeId;

            if (!Int32.TryParse(subTaskTypeId, out _subTaskTypeId))
            {
                return Json("");
            }

            string _subTaskTypeName;
            string _subTaskId;
            string _taskTypeColor;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                SubTaskType subTaskType = db.SubTaskTypes.Where(x => x.Id == _subTaskTypeId).FirstOrDefault();
                _subTaskTypeName = subTaskType.Name;
                _subTaskId = subTaskType.Id.ToString();
                _taskTypeColor = db.TaskTypes.Where(x => x.Id == subTaskType.TaskTypeId).FirstOrDefault().HexColor;

            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<WorksheetGenerator.Data.Task> tasks = db.Tasks.Where(x => x.SubTaskTypeId == _subTaskTypeId && x.Activated == true).ToList();
                TasksOfSubType tasksOfSubType = new TasksOfSubType();
                tasksOfSubType.Id = _subTaskId;
                tasksOfSubType.Tasks = tasks;
                tasksOfSubType.SubTaskTypeName = _subTaskTypeName;
                tasksOfSubType.Color = _taskTypeColor;

                List<TasksOfSubType> tasksOfSubTypes = new List<TasksOfSubType>();
                tasksOfSubTypes.Add(tasksOfSubType);
                
                return Json(tasksOfSubType);
            }
        }
        
        [HttpPost]
        public IActionResult ShowTaskInfo(string taskId)
        {
            int _taskId;

            if (!Int32.TryParse(taskId, out _taskId))
            {
                return Json("");
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                WorksheetGenerator.Data.Task t = db.Tasks.Where(x => x.Id == _taskId).FirstOrDefault();
    
                var v = new { TaskName = t.Name, TaskDescription = t.Description };
                return Json(v);
            }
        }

        [HttpGet]
        public IActionResult GetTaskSpecifications(string obj)
        {
            int[] taskIds = JsonConvert.DeserializeObject<int[]>(obj);

            if (taskIds == null) {

            }

            if (taskIds.Length == 0) {
            
            }
            List<SpecificationModel> specificationsModels = new List<SpecificationModel>();

            foreach (int taskId in taskIds)
            {
                List<Specification> specifications = new List<Specification>();
                string taskName = "";
                byte[] previewImage;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    specifications = db.Specifications.ToList().OrderBy(x => x.Position).ToList();

                    taskName = db.Tasks.Where(x => x.Id == taskId).FirstOrDefault().Name;
                    previewImage = db.Tasks.Where(x => x.Id == taskId).FirstOrDefault().PreviewImage;
                }

                SpecificationModel specificationModel = new SpecificationModel();
                specificationModel.TaskId = taskId;
                specificationModel.TaskName = taskName;
                specificationModel.PreviewImage = previewImage;

                foreach (Specification specification in specifications)
                {
                    if(specification.TaskId != taskId)
                    {
                        continue;
                    }

                    List<Option> options = new List<Option>();

                    using(ApplicationDbContext db = new ApplicationDbContext())
                    {
                        foreach (Option option in db.Options)
                        {
                            if (option.SpecificationId == specification.Id)
                            {
                                options.Add(option);
                            }
                        }

                        HTML_SpecificationType hTML_SpecificationType = db.HTML_SpecificationTypes.Where(x => x.Id == specification.HTML_SpecificationTypeId).FirstOrDefault();                      

                        TaskSpecification taskSpecification = new TaskSpecification();
                        taskSpecification.Specification = specification;
                        taskSpecification.Options = options;
                        taskSpecification.HTML_SpecificationType = hTML_SpecificationType;

                        specificationModel.TaskSpecifications.Add(taskSpecification);


                    }
                }
                specificationsModels.Add(specificationModel);

            }

            string jsonReturn = JsonConvert.SerializeObject(specificationsModels);

            return Json(jsonReturn);
        }

        private IList<SelectListItem> GetTaskTypeList()
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.TaskTypes.ToList();
                List<SelectListItem> taskTypeList = new List<SelectListItem>();

                foreach (TaskType tType in db.TaskTypes)
                {
                    taskTypeList.Add(new SelectListItem { Text = tType.Name, Value = tType.Id.ToString() });
                }

                return taskTypeList;
            }    
        }

        private IList<SelectListItem> GetTaskTypeList(int subjectId)
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<TaskType> taskTypes = db.TaskTypes.Where(x => x.SubjectId == subjectId).ToList();
                List<SelectListItem> taskTypeList = new List<SelectListItem>();

                bool taskTypeHasActiveTasks = false;
                foreach (TaskType tType in taskTypes)
                {
                    List<SubTaskType> subTaskTypeList = db.SubTaskTypes.Where(x => x.TaskTypeId == tType.Id).ToList();
                    
                    foreach (SubTaskType subTaskType in subTaskTypeList)
                    {
                        List<Task> tasks = db.Tasks.Where(x => x.SubTaskTypeId == subTaskType.Id).ToList();

                        foreach (Task task in tasks)
                        {
                            if(task.Activated == true)
                            {
                                taskTypeList.Add(new SelectListItem { Text = tType.Name, Value = tType.Id.ToString() });
                                taskTypeHasActiveTasks = true;
                                break;
                            }
                        }

                        if (taskTypeHasActiveTasks)
                        {
                            break;
                        }

                    }

                }

                return taskTypeList;
            }
        }

        private IList<SelectListItem> GetSubjectList()
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Subjects.ToList();
                List<SelectListItem> subjectList = new List<SelectListItem>();

                foreach (Subject subj in db.Subjects)
                {
                    subjectList.Add(new SelectListItem { Text = subj.Name, Value = subj.Id.ToString() });
                }

                return subjectList;
            }
        }

        private IList<SelectListItem> GetClassesList()
        {

            ApplicationDbContext db = new ApplicationDbContext();

            List<SelectListItem> classList = new List<SelectListItem>();

            foreach (Class c in db.Classes.ToList())
            {
                classList.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            }

            return classList;

        }

        private IList<SelectListItem> GetTasksList()
        {

            ApplicationDbContext db = new ApplicationDbContext();

            List<SelectListItem> taskList = new List<SelectListItem>();

            foreach (WorksheetGenerator.Data.Task t in db.Tasks.ToList())
            {
                taskList.Add(new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
            }

            return taskList;

        }

        /*
        private void replaceText(IEnumerable<Paragraph> paras, string searchString, string replaceString)
        {
            foreach (var para in paras)
            {
                foreach (var run in para.Elements<Run>())
                {
                    foreach (var text in run.Elements<Text>())
                    {
                        if (text.Text.Contains(searchString))
                        {
                            text.Text = text.Text.Replace(searchString, replaceString);
                        }
                    }
                }
            }
        }
        

        public void Word_CreateZeitformAnhandSatz(HeaderPart headerPart, MainDocumentPart mainPart, Document document1, List<ZeitformAnhandSatz> zeitenDesVerbens)
        {
            List<BulletListElement> bulletList = new List<BulletListElement>();

            foreach (ZeitformAnhandSatz _zeitenDesVerbens in zeitenDesVerbens)
            {
                BulletListElement bulletListElement = new BulletListElement();
                bulletListElement.Text = _zeitenDesVerbens.Satz;
                bulletListElement.Font = "Arial";
                bulletList.Add(bulletListElement);
            }

            _word.AddTextParagraph(mainPart, document1, $"Aufgabe 1: Zeiten des Verbs", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddTextParagraph(mainPart, document1, "Unterstreiche erst das Verb oder die Verben. Bestimme dann, in welcher Zeit die nächsten Sätze stehen. (Präsens, Perfekt, Präteritum, Plusquamperfekt oder Futur).", false, "Arial");
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");
            _word.AddBulletList(mainPart, document1, bulletList);
            _word.AddTextParagraph(mainPart, document1, "", true, "Arial");

        }
        */
    }
}
