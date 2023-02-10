using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.Admin;


namespace WorksheetGenerator.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();

            dashboardViewModel.WorksheetCount = GetWorksheetCount();
            dashboardViewModel.TaskCount = GetTaskCount();
            dashboardViewModel.UserCount = GetUserCount();

            List<Worksheet> worksheets = new List<Worksheet>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                int last = 5;

                if(db.Worksheets.ToList().Count <= last)
                {
                    for (int i = 0; i < db.Worksheets.ToList().Count; i++)
                    {
                        worksheets.Add(db.Worksheets.ToList().ElementAt(i));
                    }
                }
                else
                {
                    for (int i = (db.Worksheets.ToList().Count - last); i < db.Worksheets.ToList().Count; i++)
                    {
                        worksheets.Add(db.Worksheets.ToList().ElementAt(i));
                    }
                }
            }

            dashboardViewModel.Worksheets = worksheets;
            dashboardViewModel.Users = _userManager.Users.ToList();

            return View(dashboardViewModel);
        }

        private int GetWorksheetCount()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Worksheets.ToList().Count;              
            }
        }

        private int GetTaskCount()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Tasks.ToList().Count;
            }
        }

        private int GetUserCount()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                return _userManager.Users.Count();

            }
        }
    }
}
