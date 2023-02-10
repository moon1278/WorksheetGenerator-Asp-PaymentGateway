using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.SubTaskType;
using WorksheetGenerator.Models.SubTaskType.Helper_DetailsSubTaskTypeModel;
using Task = WorksheetGenerator.Data.Task;

namespace WorksheetGenerator.Controllers
{
    [Authorize]
    public class SubTaskTypeController : Controller
    {
        [HttpGet]
        public IActionResult Index(string javaScriptToRun)
        {
            List<SubTaskType> subTasks = new List<SubTaskType>();
            List<DetailsSubTaskTypeModel_Element> detailsSubTaskTypeModels = new List<DetailsSubTaskTypeModel_Element>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                subTasks = db.SubTaskTypes.ToList();

            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                foreach (SubTaskType subTask in subTasks)
                {
                    DetailsSubTaskTypeModel_Element detailsSubTaskTypeModel = new DetailsSubTaskTypeModel_Element();
                    detailsSubTaskTypeModel.Id = subTask.Id;
                    detailsSubTaskTypeModel.Name = subTask.Name;
                    detailsSubTaskTypeModel.Description = subTask.Description;

                    detailsSubTaskTypeModel.TaskType = db.TaskTypes.ToList().Where(x => x.Id == subTask.TaskTypeId).FirstOrDefault().Name;
                    detailsSubTaskTypeModels.Add(detailsSubTaskTypeModel);
                }
            }

            DetailsSubTaskTypeModel model = new DetailsSubTaskTypeModel();
            model.DetailsSubTaskTypeModels = detailsSubTaskTypeModels;

            if(javaScriptToRun != null)
            {
                model.JavascriptToRun = javaScriptToRun;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateSubTaskTypeModel model = new CreateSubTaskTypeModel();
            model.TaskTypes = GetAllTaskTypeList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateSubTaskTypeModel createSubTaskTypeModel)
        {
            int id;

            SubTaskType subTaskType = new SubTaskType();
            subTaskType.Name = createSubTaskTypeModel.Name;
            subTaskType.Description = createSubTaskTypeModel.Description;
            subTaskType.TaskTypeId = (int) createSubTaskTypeModel.SelectedTaskType;

            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                db.SubTaskTypes.Add(subTaskType);
                db.SaveChanges();
                id = subTaskType.Id;
            }

            return RedirectToAction("Index", "SubTaskType", new { javaScriptToRun = "ShowSuccessMessage('Der SubTaskType (Id " + id + ") wurde erfolgreich erstellt.')" });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            SubTaskType subTaskType = new SubTaskType();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                subTaskType = db.SubTaskTypes.Where(x => x.Id == id).FirstOrDefault();
            }

            if(subTaskType == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            EditSubTaskTypeModel editSubTaskTypeModel = new EditSubTaskTypeModel();
            editSubTaskTypeModel.Id = id;
            editSubTaskTypeModel.Name = subTaskType.Name;
            editSubTaskTypeModel.Description = subTaskType.Description;
            editSubTaskTypeModel.TaskTypes = GetAllTaskTypeList();
            editSubTaskTypeModel.SelectedTaskType = subTaskType.TaskTypeId;

            return View(editSubTaskTypeModel);
        }

        [HttpPost]
        public ActionResult Edit(EditSubTaskTypeModel model)
        {
            int id;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                SubTaskType subTaskType = db.SubTaskTypes.Where (x => x.Id == model.Id).FirstOrDefault();
                subTaskType.Name = model.Name;
                subTaskType.Description = model.Description;
                subTaskType.TaskTypeId = (int) model.SelectedTaskType;
                db.SaveChanges();

                id = subTaskType.Id;
            }

            return RedirectToAction("Index", "SubTaskType", new { javaScriptToRun = "ShowSuccessMessage('Der SubTaskType (Id "+ id+") wurde erfolgreich geupdated.')" });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            SubTaskType subTaskType = new SubTaskType();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                subTaskType = db.SubTaskTypes.Where(x => x.Id == id).FirstOrDefault();
            }

            if (subTaskType == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            DeleteSubTaskTypeModel deleteSubTaskTypeModel = new DeleteSubTaskTypeModel();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                deleteSubTaskTypeModel.Id = id;
                deleteSubTaskTypeModel.Name = subTaskType.Name;
                deleteSubTaskTypeModel.Description = subTaskType.Description;
                deleteSubTaskTypeModel.TaskType = db.TaskTypes.ToList().Where(x => x.Id == subTaskType.TaskTypeId).FirstOrDefault().Name;
                deleteSubTaskTypeModel.Tasks = db.Tasks.Where(x => x.SubTaskTypeId == id).ToList();               
            }

            return View(deleteSubTaskTypeModel);
        }

        [HttpPost]
        public ActionResult Delete(DeleteSubTaskTypeModel deleteSubTaskTypeModel)
        {
            int id = deleteSubTaskTypeModel.Id;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<Task> tasks = db.Tasks.Where(x => x.SubTaskTypeId == deleteSubTaskTypeModel.Id).ToList();
                db.Tasks.RemoveRange(tasks);

                db.Remove(db.SubTaskTypes.Where(x => x.Id == deleteSubTaskTypeModel.Id).FirstOrDefault());

                db.SaveChanges();

            }
            return RedirectToAction("Index", "SubTaskType", new { javaScriptToRun = "ShowSuccessMessage('Der SubTaskType (Id " + id + ") wurde erfolgreich gelöscht.')" });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            SubTaskType subTaskType = new SubTaskType();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                subTaskType = db.SubTaskTypes.Where(x => x.Id == id).FirstOrDefault();
            }

            if (subTaskType == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            DetailsSubTaskTypeModel_Element detailsSubTaskTypeModel_Element = new DetailsSubTaskTypeModel_Element();
            detailsSubTaskTypeModel_Element.Id = id;
            detailsSubTaskTypeModel_Element.Name = subTaskType.Name;
            detailsSubTaskTypeModel_Element.Description = subTaskType.Description;

            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                detailsSubTaskTypeModel_Element.TaskType = db.TaskTypes.ToList().Where(x => x.Id == subTaskType.TaskTypeId).FirstOrDefault().Name;
                detailsSubTaskTypeModel_Element.Tasks = db.Tasks.Where(x => x.SubTaskTypeId == id).ToList();
            }


            return View(detailsSubTaskTypeModel_Element);
        }

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

        private IList<SelectListItem> GetAllTaskTypeList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<TaskType> taskTypes = db.TaskTypes.ToList();
                List<SelectListItem> taskTypeList = new List<SelectListItem>();

                foreach (TaskType tType in taskTypes)
                {
                    taskTypeList.Add(new SelectListItem { Text = tType.Name, Value = tType.Id.ToString() });
                }

                return taskTypeList;
            }
        }

    }
}
