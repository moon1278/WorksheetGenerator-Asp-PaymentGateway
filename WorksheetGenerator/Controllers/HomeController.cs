using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using WorksheetGenerator.Data;
using WorksheetGenerator.Helper.Logging;
using WorksheetGenerator.Helper.Word;
using WorksheetGenerator.Models;
using WorksheetGenerator.Models.WorksheetModels.TaskModels;

namespace WorksheetGenerator.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly LogHelper _logger = new LogHelper();

        public HomeController(ILogger<HomeController> logger)
        {
           // _logger = logger;
        }

        public IActionResult Index()
        {

          

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}