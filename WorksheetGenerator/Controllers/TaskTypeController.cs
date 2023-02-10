using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.TaskType.Helper_DetailsTaskTypeModel;
using WorksheetGenerator.Models.TaskType;
using Microsoft.AspNetCore.Authorization;
using Task = WorksheetGenerator.Data.Task;

namespace WorksheetGenerator.Controllers
{
    [Authorize]
    public class TaskTypeController : Controller
    {
        [HttpGet]
        public IActionResult Index(string javaScriptToRun)
        {
            List<TaskType> taskTypes = new List<TaskType>();
            List<DetailsTaskTypeModel_Element> detailsTaskTypeModels = new List<DetailsTaskTypeModel_Element>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                taskTypes = db.TaskTypes.ToList();

            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                foreach (TaskType taskType in taskTypes)
                {
                    DetailsTaskTypeModel_Element detailsTaskTypeModel = new DetailsTaskTypeModel_Element();
                    detailsTaskTypeModel.Id = taskType.Id;
                    detailsTaskTypeModel.Name = taskType.Name;
                    detailsTaskTypeModel.Description = taskType.Description;
                    detailsTaskTypeModel.HexColor = taskType.HexColor;

                    detailsTaskTypeModel.Subject = db.Subjects.ToList().Where(x => x.Id == taskType.SubjectId).FirstOrDefault().Name;
                    detailsTaskTypeModels.Add(detailsTaskTypeModel);
                }
            }

            DetailsTaskTypeModel model = new DetailsTaskTypeModel();
            model.DetailsTaskTypeModels = detailsTaskTypeModels;

            if(javaScriptToRun != null)
            {
                model.JavascriptToRun = javaScriptToRun;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateTaskTypeModel model = new CreateTaskTypeModel();
            model.Subjects = GetAllSubjectList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateTaskTypeModel createTaskTypeModel)
        {
            int id;

            TaskType taskType = new TaskType();
            taskType.Name = createTaskTypeModel.Name;
            taskType.Description = createTaskTypeModel.Description;
            taskType.SubjectId = (int) createTaskTypeModel.SelectedSubject;
            taskType.HexColor = createTaskTypeModel.HexColor;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.TaskTypes.Add(taskType);
                db.SaveChanges();
                id = taskType.Id;
            }

            return RedirectToAction("Index", "TaskType", new { javaScriptToRun = "ShowSuccessMessage('Der TaskType (Id " + id + ") wurde erfolgreich erstellt.')" });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            TaskType taskType = new TaskType();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                taskType = db.TaskTypes.Where(x => x.Id == id).FirstOrDefault();
            }

            if(taskType == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            EditTaskTypeModel editTaskTypeModel = new EditTaskTypeModel();
            editTaskTypeModel.Id = id;
            editTaskTypeModel.Name = taskType.Name;
            editTaskTypeModel.Description = taskType.Description;
            editTaskTypeModel.TaskTypes = GetAllSubjectList();
            editTaskTypeModel.SelectedTaskType = taskType.SubjectId;
            editTaskTypeModel.HexColor = taskType.HexColor;


            return View(editTaskTypeModel);
        }

        [HttpPost]
        public ActionResult Edit(EditTaskTypeModel model)
        {
            int id;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                TaskType taskType = db.TaskTypes.Where (x => x.Id == model.Id).FirstOrDefault();
                taskType.Name = model.Name;
                taskType.Description = model.Description;
                taskType.SubjectId = (int) model.SelectedTaskType;
                taskType.HexColor = model.HexColor;

                db.SaveChanges();

                id = taskType.Id;
            }

            return RedirectToAction("Index", "TaskType", new { javaScriptToRun = "ShowSuccessMessage('Der TaskType (Id "+ id+") wurde erfolgreich geupdated.')" });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            TaskType taskType = new TaskType();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                taskType = db.TaskTypes.Where(x => x.Id == id).FirstOrDefault();
            }

            if (taskType == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            DeleteTaskTypeModel deleteTaskTypeModel = new DeleteTaskTypeModel();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                deleteTaskTypeModel.Id = id;
                deleteTaskTypeModel.Name = taskType.Name;
                deleteTaskTypeModel.Description = taskType.Description;
                deleteTaskTypeModel.HexColor = taskType.HexColor;

                List<SubTaskType> subTaskTypes = new List<SubTaskType>();
                List<Task> tasks = new List<Task>();
                deleteTaskTypeModel.SubTaskTypes = subTaskTypes;

                subTaskTypes = db.SubTaskTypes.Where(x => x.TaskTypeId == id).ToList();
                
                foreach(SubTaskType subTaskType in subTaskTypes)
                {
                    subTaskType.Tasks = db.Tasks.Where(x => x.SubTaskTypeId == subTaskType.Id).ToList();
                }

                if (subTaskTypes.Count != 0)
                {             
                    deleteTaskTypeModel.SubTaskTypes = subTaskTypes;
                }

                deleteTaskTypeModel.Subject = db.Subjects.ToList().Where(x => x.Id == taskType.SubjectId).FirstOrDefault().Name;

                


            }

            return View(deleteTaskTypeModel);
        }

        [HttpPost]
        public ActionResult Delete(DeleteTaskTypeModel deleteTaskTypeModel)
        {
            int id = deleteTaskTypeModel.Id;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<SubTaskType> subTaskTypes = db.SubTaskTypes.Where(x => x.TaskTypeId == deleteTaskTypeModel.Id).ToList();
                db.SubTaskTypes.RemoveRange(subTaskTypes);

                
                db.Remove(db.TaskTypes.Where(x => x.Id == deleteTaskTypeModel.Id).FirstOrDefault());

                db.SaveChanges();

                

            }
            return RedirectToAction("Index", "TaskType", new { javaScriptToRun = "ShowSuccessMessage('Der TaskType (Id " + id + ") wurde erfolgreich gelöscht.')" });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            TaskType taskType = new TaskType();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                taskType = db.TaskTypes.Where(x => x.Id == id).FirstOrDefault();
            }

            if (taskType == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            DetailsTaskTypeModel_Element detailsTaskTypeModel_Element = new DetailsTaskTypeModel_Element();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                detailsTaskTypeModel_Element.Id = id;
                detailsTaskTypeModel_Element.Name = taskType.Name;
                detailsTaskTypeModel_Element.Description = taskType.Description;
                detailsTaskTypeModel_Element.HexColor = taskType.HexColor;

                List<SubTaskType> subTaskTypes = new List<SubTaskType>();
                List<Task> tasks = new List<Task>();
                detailsTaskTypeModel_Element.SubTaskTypes = subTaskTypes;

                subTaskTypes = db.SubTaskTypes.Where(x => x.TaskTypeId == id).ToList();

                foreach (SubTaskType subTaskType in subTaskTypes)
                {
                    //subTaskType.Tasks = db.Tasks.Where(x => x.SubTaskTypeId == subTaskType.Id).ToList();
                }

                if (subTaskTypes.Count != 0)
                {
                    detailsTaskTypeModel_Element.SubTaskTypes = subTaskTypes;
                }



            }


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                detailsTaskTypeModel_Element.Subject = db.Subjects.ToList().Where(x => x.Id == taskType.SubjectId).FirstOrDefault().Name;

            }

            return View(detailsTaskTypeModel_Element);
        }

        /*
        private IList<SelectListItem> GetTaskTypeList(int subjectId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<TaskType> taskTypes = db.TaskTypes.Where(x => x.SubjectId == subjectId).ToList();
                List<SelectListItem> taskTypeList = new List<SelectListItem>();

                foreach (TaskType tType in taskTypes)
                {
                    taskTypeList.Add(new SelectListItem { Text = tType.Name, Value = tType.Id.ToString() });
                }

                return taskTypeList;
            }
        }
        */
        private IList<SelectListItem> GetAllSubjectList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Subject> subjects = db.Subjects.ToList();
                List<SelectListItem> subjectsList = new List<SelectListItem>();

                foreach (Subject subject in subjects)
                {
                    subjectsList.Add(new SelectListItem { Text = subject.Name, Value = subject.Id.ToString() });
                }

                return subjectsList;
            }
        }

    }
}
