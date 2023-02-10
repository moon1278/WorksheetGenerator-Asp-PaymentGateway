using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Net;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.JSON;
using WorksheetGenerator.Models.Task;
using WorksheetGenerator.Models.Task.Helper_DetailsTaskModel;
using Task = WorksheetGenerator.Data.Task;

namespace WorksheetGenerator.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TaskController : Controller
    {
      

        [HttpGet]
        public IActionResult Index(string javaScriptToRun)
        {
            List<Task> tasks = new List<Task>();
            List<DetailsTaskModel_Element> detailsTaskModels = new List<DetailsTaskModel_Element>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                tasks = db.Tasks.ToList();
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (Task task in tasks)
                {
                    DetailsTaskModel_Element detailsTaskModel = new DetailsTaskModel_Element();
                    detailsTaskModel.Id = task.Id;
                    detailsTaskModel.Name = task.Name;
                    detailsTaskModel.Description = task.Description;
                    detailsTaskModel.Query = task.Query;
                    detailsTaskModel.Activated = task.Activated;

                    detailsTaskModel.SubTaskType = db.SubTaskTypes.ToList().Where(x => x.Id == task.SubTaskTypeId).FirstOrDefault().Name;
                    detailsTaskModels.Add(detailsTaskModel);
                }
            }

            DetailsTaskModel model = new DetailsTaskModel();
            model.DetailsTaskModels = detailsTaskModels;

            if(javaScriptToRun != null)
            {
                model.JavascriptToRun = javaScriptToRun;
            }

            return View(model);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            CreateTaskModel model = new CreateTaskModel();
            model.SubTaskTypes = GetAllSubTaskTypesList();
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateTaskModel createTaskModel, IFormFile image)
        {
            int id;

            // Convert the image to a byte array
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                imageData = memoryStream.ToArray();
            }

            Task task = new Task();
            task.Name = createTaskModel.Name;
            task.Description = createTaskModel.Description;
            task.SubTaskTypeId = (int)createTaskModel.SelectedSubTaskType;
            task.Query = createTaskModel.Query;
            task.Activated = createTaskModel.Activated;
            task.PreviewImage = imageData;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                id = task.Id;
            }

            return RedirectToAction("Index", "Task", new { javaScriptToRun = "ShowSuccessMessage('Der Task (Id " + id + ") wurde erfolgreich erstellt.')" });
        }

        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Task task = new Task();

            List<HTML_SpecificationType> options = new List<HTML_SpecificationType>();
            List<Specifictions_Option> specifications = new List<Specifictions_Option>();
            int selected_HTML_SpecificationType;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                task = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
                options = db.HTML_SpecificationTypes.ToList();

                foreach (Specification spec in db.Specifications.Where(x => x.TaskId == id).ToList())
                {
                    Specifictions_Option specifictions_Option = new Specifictions_Option();
                    specifictions_Option.HTML_SpecificationTypeId = spec.HTML_SpecificationTypeId;
                    specifictions_Option.Dynamic_Replace_Text = spec.Dynamic_Replace_Text;
                    specifictions_Option.TaskId = spec.TaskId;
                    specifictions_Option.Name = spec.Name;
                    specifictions_Option.Description = spec.Description;
                    specifictions_Option.Id = spec.Id;
                    specifictions_Option.Position = spec.Position;

                    specifications.Add(specifictions_Option);
                }
            }

            if (task == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            EditTaskModel editTaskModel = new EditTaskModel();
            editTaskModel.Id = id;
            editTaskModel.Name = task.Name;
            editTaskModel.Description = task.Description;
            editTaskModel.SubTaskTypes = GetAllSubTaskTypesList();
            editTaskModel.SelectedSubTaskType = task.SubTaskTypeId;
            editTaskModel.Query = task.Query;
            editTaskModel.Activated = task.Activated;
            editTaskModel.PreviewImage = task.PreviewImage;
            editTaskModel.HTML_SpecificationTypes = options;
            
            editTaskModel.Specifications = specifications.OrderBy(x => x.Position).ToList();

            return View(editTaskModel);
        }

        [HttpPost]
        public ActionResult Edit(EditTaskModel model, IFormFile previewImage)
        {
            int taskId;
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Task task = db.Tasks.Where (x => x.Id == model.Id).FirstOrDefault();
                task.Name = model.Name;
                task.Description = model.Description;
                task.SubTaskTypeId = (int) model.SelectedSubTaskType;
                task.Query = model.Query;
                task.Activated = model.Activated;

                if(previewImage != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        previewImage.CopyTo(stream);
                        task.PreviewImage = stream.ToArray();
                    }
                }

                foreach(Specifictions_Option spec_option in model.Specifications)
                {
                    //Specification does not exist in db, create new spec
                    if(spec_option.Id == 0)
                    {
                        Specification specification = new Specification();
                        specification.HTML_SpecificationTypeId = spec_option.Selected_HTML_SpecificationTypeId;
                        specification.TaskId = model.Id;
                        specification.Dynamic_Replace_Text = spec_option.Dynamic_Replace_Text;
                        specification.Description = spec_option.Description;
                        specification.Name = spec_option.Name;
                        specification.Position = spec_option.Position;

                        db.Specifications.Add(specification);
                    }
                    else
                    {
                        Specification sp = db.Specifications.Where(x => x.Id == spec_option.Id).FirstOrDefault();

                        sp.HTML_SpecificationTypeId = spec_option.Selected_HTML_SpecificationTypeId;
                        sp.Dynamic_Replace_Text = spec_option.Dynamic_Replace_Text;
                        sp.Name = spec_option.Name;
                        sp.Description = spec_option.Description;
                       
                    }
                }
                
                db.SaveChanges();

                taskId = task.Id;
            }

            return RedirectToAction("Index", "Task", new { javaScriptToRun = "ShowSuccessMessage('Der Task (Id "+ taskId+") wurde erfolgreich geupdated.')" });
        }

        [HttpPost]
        public IActionResult DeleteSpecification(int id)
        {
            Specification spec = new Specification();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                spec = db.Specifications.Where(x => x.Id == id).FirstOrDefault();

                if(spec != null)
                {

                    db.Specifications.Remove(spec);
                    db.SaveChanges();
                }

                
            }

            return Json("Success");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Task task = new Task();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                task = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
            }

            if (task == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            DeleteTaskModel deleteTaskModel = new DeleteTaskModel();

            deleteTaskModel.Id = id;
            deleteTaskModel.Name = task.Name;
            deleteTaskModel.Description = task.Description;
            deleteTaskModel.Query = task.Query;
            deleteTaskModel.Activated = task.Activated;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                deleteTaskModel.SubTaskType = db.SubTaskTypes.ToList().Where(x => x.Id == task.SubTaskTypeId).FirstOrDefault().Name;

            }

            return View(deleteTaskModel);
        }

        [HttpPost]
        public ActionResult Delete(DeleteTaskModel deleteTaskModel)
        {
            int id = deleteTaskModel.Id;

            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Tasks.Remove(db.Tasks.Where(x => x.Id == id).FirstOrDefault());
                db.SaveChanges();
            }

            return View("Index");
        }
        
       
        [HttpGet]
        public IActionResult Details(int id)
        {
            Task task = new Task();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                task = db.Tasks.Where(x => x.Id == id).FirstOrDefault();
            }

            if (task == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            DetailsTaskModel_Element detailsTaskModel_Element = new DetailsTaskModel_Element();
            detailsTaskModel_Element.Id = id;
            detailsTaskModel_Element.Name = task.Name;
            detailsTaskModel_Element.Description = task.Description;
            detailsTaskModel_Element.Query = task.Query;
            detailsTaskModel_Element.Activated = task.Activated;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                detailsTaskModel_Element.SubTaskType = db.SubTaskTypes.ToList().Where(x => x.Id == task.SubTaskTypeId).FirstOrDefault().Name;

            }

            return View(detailsTaskModel_Element);
        }


        public IActionResult AddJson()
        {
            var viewModel = new CreateJSONClassViewModel();
            viewModel.PropertyTypes = GeneratePropertyTypes();

            return View(viewModel);
        }

        public IActionResult GetSelectListItems(int specId)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                Specification spec = db.Specifications.Where(x => x.Id == specId).FirstOrDefault();

                if(spec != null)
                {
                    List<Option> options = db.Options.Where(x => x.SpecificationId == specId).ToList();

                    return Json(options);
                }

            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content("Error", MediaTypeNames.Text.Plain);
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

        private List<SelectListItem> GeneratePropertyTypes()
        {
            // Generate a list of SelectListItem objects with the C# data types as options
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "string", Text = "string" },
            new SelectListItem { Value = "int", Text = "int" },
            new SelectListItem { Value = "byte", Text = "byte" },
            // Add additional C# data types as needed
        };
        }


    }
}
