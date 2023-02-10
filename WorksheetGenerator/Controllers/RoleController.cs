using Microsoft.AspNetCore.Mvc;
using WorksheetGenerator.Data;
using WorksheetGenerator.Models.Task.Helper_DetailsTaskModel;
using WorksheetGenerator.Models.Task;
using Microsoft.AspNetCore.Identity;
using WorksheetGenerator.Models.Role;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WorksheetGenerator.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
       

         [HttpPost]
         public ActionResult Create(CreateRoleViewModel createRoleViewModel)
         {
             IdentityRole identityRole = new IdentityRole();
             identityRole.Name = createRoleViewModel.Name;


            IdentityResult result = _roleManager.CreateAsync(identityRole).Result;

            if (result.Succeeded)
            {
                return Redirect("Index");
            }
            else
            {
                return Redirect("Index");

            }

        }

  
        [HttpGet]
        public IActionResult Edit(string id)
        {
            EditRoleViewModel role = new EditRoleViewModel();

            var r = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();
        
            if (r == null)
            {
                return View("Error");
                //TODO Add Error View
            }

            EditRoleViewModel editRoleViewModel = new EditRoleViewModel();
            editRoleViewModel.Id = r.Id;
            editRoleViewModel.Name = r.Name;
            
            return View(editRoleViewModel);
        }

        
        [HttpPost]
        public ActionResult Edit(EditRoleViewModel model)
        {
            var role = _roleManager.Roles.Where(x => x.Id == model.Id).FirstOrDefault();

            if(role == null)
            {
                return View("Error");
            }

            role.Name = model.Name;

            IdentityResult result = _roleManager.UpdateAsync(role).Result;

            if (result.Succeeded)
            {
                RedirectToAction("Index");
            }
            else
            {
                RedirectToAction("Index");

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var r = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();

            if(r == null)
            {
                return View("Error");
            }
            DeleteRoleViewModel deleteRoleModel = new DeleteRoleViewModel();

            deleteRoleModel.Id = r.Id;
            deleteRoleModel.Name = r.Name;

            // Get a list of users in the role
            var usersWithPermission = _userManager.GetUsersInRoleAsync(r.Name).Result;

            // Then get a list of the ids of these users
            var idsWithPermission = usersWithPermission.Select(u => u.Id);

            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                // Now get the users in our database with the same ids
                var users = db.Users.Where(u => idsWithPermission.Contains(u.Id)).ToList();
                
                if(users != null && users.Count > 0)
                {
                    deleteRoleModel.UsersInRole = users;
                }
            }

            return View(deleteRoleModel);
        }

  
         [HttpPost]
         public ActionResult Delete(DeleteRoleViewModel deleteRoleViewModel)
         {
            var r = _roleManager.Roles.Where(x => x.Id == deleteRoleViewModel.Id).FirstOrDefault();

            if(r == null)
            {
                return View("Error");
            }

            IdentityResult result = _roleManager.DeleteAsync(r).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");

            }

            return View("Index");
         }

        
         [HttpGet]
         public IActionResult Details(string id)
         {
            var r = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();

            if (r == null)
            {
                return View("Error");
            }
            DetailsRoleViewModel detailsRoleViewModel = new DetailsRoleViewModel();

            detailsRoleViewModel.Id = r.Id;
            detailsRoleViewModel.Name = r.Name;

            // Get a list of users in the role
            var usersWithPermission = _userManager.GetUsersInRoleAsync(r.Name).Result;

            // Then get a list of the ids of these users
            var idsWithPermission = usersWithPermission.Select(u => u.Id);

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                // Now get the users in our database with the same ids
                var users = db.Users.Where(u => idsWithPermission.Contains(u.Id)).ToList();

                if (users != null && users.Count > 0)
                {
                    detailsRoleViewModel.UsersInRole = users;
                }
            }

            return View(detailsRoleViewModel);
        }

        
        [HttpGet]
        public async Task<IActionResult> AddUserAsync(string id)
        {
        var r = _roleManager.Roles.Where(x => x.Id == id).FirstOrDefault();

        if (r == null)
        {
            return View("Error");
        }
        AddUserToRoleViewModel addUserToRoleViewModel = new AddUserToRoleViewModel();

        addUserToRoleViewModel.RoleId = r.Id;
        addUserToRoleViewModel.RoleName = r.Name;

        var usersInRole = await _userManager.GetUsersInRoleAsync(r.Name);

        using(ApplicationDbContext context = new ApplicationDbContext())
        {
            var allUsers = await context.Users.ToListAsync();

            var usersNotInRole = new List<IdentityUser>();
            foreach (var u in allUsers)
            {
                if (!usersInRole.Any(x => x.Id == u.Id))
                {
                    usersNotInRole.Add(u);
                }
            }

            addUserToRoleViewModel.UsersNotInRole = usersNotInRole;

        }

          
        return View(addUserToRoleViewModel);
    }

        [HttpPost]
        public async Task<IActionResult> AddUser(List<string> userList, string roleId)
        {
            // Create a new database context object
            using (var context = new ApplicationDbContext())
            {
                // Fetch the role with the specified ID
                var role = context.Roles.FirstOrDefault(r => r.Id == roleId);
                if (role == null)
                {
                    // Handle error if role does not exist
                }

                // Loop through the list of user IDs
                foreach (var userId in userList)
                {
                    // Fetch the user with the specified ID
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null)
                    {
                        // Handle error if user does not exist
                    }

                    // Add the user to the role
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                // Save changes to the database
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    


         [HttpPost]
        public async Task<ActionResult> RemoveUserAsync(string userId, string roleName)
        {
            var userInDb = _userManager.Users.FirstOrDefault(user => user.Id == userId);
            if (userInDb == null)
                return NotFound();

            //get user's assigned roles
            IList<string> userRoles = await _userManager.GetRolesAsync(userInDb);

            //check for role to be removed
            var roleToRemove = userRoles.FirstOrDefault(role => role.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));
            if (roleToRemove == null)
                return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(userInDb, roleToRemove);

            if (result.Succeeded)
                RedirectToAction("Index");

            return BadRequest();

        }

    }
}
