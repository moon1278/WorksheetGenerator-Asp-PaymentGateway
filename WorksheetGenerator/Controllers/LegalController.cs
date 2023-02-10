using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.TaskType.Helper_DetailsTaskTypeModel;
using WorksheetGenerator.Models.TaskType;

namespace WorksheetGenerator.Controllers
{
    public class LegalController : Controller
    {


        [HttpGet]
        public IActionResult Impressum()
        {
           
            return View();
        }

        [HttpGet]
        public IActionResult DSGVO()
        {

            return View();
        }



    }
}
