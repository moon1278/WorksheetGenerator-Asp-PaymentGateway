using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.Task;
using WorksheetGenerator.Models.Task.Helper_DetailsTaskModel;
using WorksheetGenerator.Models.WorksheetManagement;
using WorksheetGenerator.Models.WorksheetManagement.Helper_DetailsWorksheetManagementModel;

namespace WorksheetGenerator.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class WorksheetManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public WorksheetManagementController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(string javaScriptToRun)
        {
            List<Worksheet> worksheets = new List<Worksheet>();
            List<DetailsWorksheetManagementModel_Element> detailsWorksheetManagementModels = new List<DetailsWorksheetManagementModel_Element>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                worksheets = db.Worksheets.ToList();
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (Worksheet worksheet in worksheets)
                {
                    DetailsWorksheetManagementModel_Element detailsWorksheetManagementkModel = new DetailsWorksheetManagementModel_Element();
                    detailsWorksheetManagementkModel.Id = worksheet.Id;
                    detailsWorksheetManagementkModel.Name = worksheet.Name;
                    detailsWorksheetManagementkModel.Created = worksheet.Created;


                    var user = _userManager.FindByIdAsync(worksheet.UserId);

                    detailsWorksheetManagementkModel.Created_User = user.Result.Email;

                    detailsWorksheetManagementModels.Add(detailsWorksheetManagementkModel);
                }
            }

            DetailsWorksheetManagementModel model = new DetailsWorksheetManagementModel();
            model.DetailsTaskModels = detailsWorksheetManagementModels;

            if(javaScriptToRun != null)
            {
                model.JavascriptToRun = javaScriptToRun;
            }

            return View(model);
        }
   
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Worksheet task = new Worksheet();

            List<string> options = new List<string>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                task = db.Worksheets.Where(x => x.Id == id).FirstOrDefault();
            }

            if(task == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            EditWorksheetManagementModel editWorksheetManagementModel = new EditWorksheetManagementModel();
            editWorksheetManagementModel.Id = id;
            editWorksheetManagementModel.Name = task.Name;
            editWorksheetManagementModel.Created = task.Created;
            editWorksheetManagementModel.Created_User = task.UserId;
      
            return View(editWorksheetManagementModel);
        }

        [HttpPost]
        public ActionResult Edit(EditWorksheetManagementModel model)
        {
            int id;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Worksheet task = db.Worksheets.Where (x => x.Id == model.Id).FirstOrDefault();
                task.Name = model.Name;
                
                db.SaveChanges();

                id = task.Id;
            }

            return RedirectToAction("Index", "WorksheetManagement", new { javaScriptToRun = "ShowSuccessMessage('Das Arbeitsblatt (Id "+ id+") wurde erfolgreich geupdated.')" });
        }

        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Worksheet task = new Worksheet();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                task = db.Worksheets.Where(x => x.Id == id).FirstOrDefault();
            }

            if (task == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            DeleteWorksheetManagementModel deleteWorksheetManagementModel = new DeleteWorksheetManagementModel();

            deleteWorksheetManagementModel.Id = id;
            deleteWorksheetManagementModel.Name = task.Name;

            return View(deleteWorksheetManagementModel);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Worksheet worksheet = db.Worksheets.Where(x => x.Id == Int32.Parse(id)).FirstOrDefault();
                db.Worksheets.Remove(worksheet);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "WorksheetManagement", new { javaScriptToRun = "ShowSuccessMessage('Das Arbeitsblatt (Id " + id + ") wurde erfolgreich gelöscht.')" });
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            Worksheet worksheet = new Worksheet();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                worksheet = db.Worksheets.Where(x => x.Id == id).FirstOrDefault();
            }

            if (worksheet == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            DetailsWorksheetManagementModel_Element detailsWorksheetManagementModel_Element = new DetailsWorksheetManagementModel_Element();
            detailsWorksheetManagementModel_Element.Id = id;
         

            return View(detailsWorksheetManagementModel_Element);
        }

        private IList<SelectListItem> GetAllSubTaskTypesList()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<SubTaskType> subTaskTypes = db.SubTaskTypes.ToList();
                List<SelectListItem> subTaskTypesList = new List<SelectListItem>();

                foreach (SubTaskType tType in subTaskTypes)
                {
                    subTaskTypesList.Add(new SelectListItem { Text = tType.Name, Value = tType.Id.ToString() });
                }

                return subTaskTypesList;
            }
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


    }
}
